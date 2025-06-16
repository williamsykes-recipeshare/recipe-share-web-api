using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using RecipeShareLibrary.Model.Settings;
using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.DBContext.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using RecipeShareWebApi.Helper;
using RecipeShareWebApi.Converters;
using RecipeShareWebApi.Middleware;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Manager.Rights.Implementation;
using RecipeShareWebApi.Services.Rights;
using RecipeShareWebApi.Services.Rights.Implementation;
using RecipeShareLibrary.Validator.Rights;
using RecipeShareLibrary.Validator.Rights.Implementation;
using Microsoft.Extensions.Options;
using RecipeShareLibrary.Manager.MasterData;
using RecipeShareLibrary.Manager.MasterData.Implementation;
using RecipeShareWebApi.Services.MasterData;
using RecipeShareWebApi.Services.MasterData.Implementation;
using RecipeShareLibrary.Validator.MasterData;
using RecipeShareLibrary.Validator.MasterData.Implementation;
using RecipeShareLibrary.Manager.Recipes;
using RecipeShareLibrary.Manager.Recipes.Implementation;
using RecipeShareWebApi.Services.Recipes;
using RecipeShareWebApi.Services.Recipes.Implementation;
using RecipeShareLibrary.Validator.Recipes;
using RecipeShareLibrary.Validator.Recipes.Implementation;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseDefaultServiceProvider((_, options) => {
            options.ValidateScopes = true;
            options.ValidateOnBuild = false;
        });

    builder.Configuration
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

    // Add services to the container.
    builder.Services.AddOptions();
    builder.Services.Configure<ApplicationSettings>(
        builder.Configuration.GetSection("ApplicationSettings"));

    var applicationSettings = new ApplicationSettings();
    builder.Configuration.GetSection("ApplicationSettings").Bind(applicationSettings);

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = applicationSettings.TokenSettings?.Issuer,
                    ValidAudience = applicationSettings.TokenSettings?.Audience,
                    IssuerSigningKey =
                        JwtSecurityKey.Create(applicationSettings.TokenSettings?.SecretKey ?? "")
                };
        });

    builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<GzipCompressionProvider>();
    });

    builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            // this constructor is overloaded.  see other overloads for options.
            options.JsonSerializerOptions.Converters.Add(new DateTimeUtcConverter());
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddScoped<IRecipeShareDbContextFactory, RecipeShareDbContextFactory>();

    if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("MySQL")))
    {
        var conString = builder.Configuration.GetConnectionString("MySQL");

        builder.Services.AddDbContextFactory<RecipeShareDbContext, RecipeShareDbContextFactory>(options =>
        {
            options.UseMySql(conString, ServerVersion.AutoDetect(conString),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
    }

    if (applicationSettings.RateLimiterSettings != null)
    {
        builder.Services.AddRateLimiter(limiterOptions =>
        {
            limiterOptions.OnRejected = (context, _) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(CultureInfo.InvariantCulture);
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.RequestServices.GetService<ILogger>()?
                    .LogWarning("OnLimiterRejected: {UserEndPoint}", GetUserEndPoint(context.HttpContext));

                return new ValueTask();
            };

            limiterOptions.AddPolicy(RateLimiterSettings.MyRateLimit, context =>
            {
                // Do not use IP or other user definable partition key
                // as this opens up DDOS and bypassing of limiter.
                var partitionKey = "anonymous user";
                if (context.User.Identity?.IsAuthenticated is true)
                {
                    partitionKey = context.User.ToString()!;
                }

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = applicationSettings.RateLimiterSettings.PermitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = applicationSettings.RateLimiterSettings.QueueLimit,
                        Window = TimeSpan.FromSeconds(applicationSettings.RateLimiterSettings.Window)
                    });
            });
        });
    }

    // Managers
    #region Managers

    #region Master Data

    builder.Services.AddScoped<IIngredientManager, IngredientManager>();

    #endregion

    #region Recipes

    builder.Services.AddScoped<IRecipeManager, RecipeManager>();

    #endregion

    #region Rights

    builder.Services.AddScoped<IUserManager, UserManager>();
    builder.Services.AddScoped<IUserTokenManager, UserTokenManager>();
    builder.Services.AddScoped<IRightManager, RightManager>();

    #endregion

    #endregion

    // Services
    #region Services

    #region Master Data

    builder.Services.AddTransient<IIngredientService, IngredientService>();

    #endregion

    #region Recipes

    builder.Services.AddTransient<IRecipeService, RecipeService>();

    #endregion

    #region Rights

    builder.Services.AddTransient<IAuthorisationService, AuthorisationService>();
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IRightService, RightService>();

    #endregion

    #endregion

    // Validators
    #region Validator

    #region Master Data

    builder.Services.AddSingleton<IIngredientValidator, IngredientValidator>();

    #endregion

    #region Master Data

    builder.Services.AddSingleton<IRecipeValidator, RecipeValidator>();

    #endregion

    #region Rights

    builder.Services.AddSingleton<IUserValidator, UserValidator>(options => new UserValidator(options.GetRequiredService<IOptions<ApplicationSettings>>().Value));

    #endregion

    #endregion

    builder.Services.AddHttpContextAccessor();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Porfolio of Evidence API", Version = "v1" });
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "Porfolio of Evidence API v2", Version = "v2" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { In = ParameterLocation.Header, Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    },
                    UnresolvedReference  = true
                }, new List<string>() }
        });
    });

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
    });

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    });

    var app = builder.Build();

    await using (var scoped = app.Services.CreateAsyncScope())
    {
        // Check if DB exists and correct
        var dbFactory = scoped.ServiceProvider.GetRequiredService<IRecipeShareDbContextFactory>();
        await using (var dBContext = dbFactory.CreateDbContext())
        {
            await dBContext.Database.EnsureCreatedAsync();
        }
    }

    app.UseResponseCompression();

    app.UseForwardedHeaders();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseCors(options =>
        {
            options.WithHeaders("accept", "content-type", "origin", "x-custom-header", "Authorization", "x-filename")
                .WithExposedHeaders("Content-Disposition")
                .AllowAnyMethod()
                .WithOrigins("http://localhost:8080"); // TODO: add deployed web app URL here as well
        });
    } else if(app.Environment.IsEnvironment("QA"))
    {
        app.UseCors(options =>
        {
            options.WithHeaders("accept", "content-type", "origin", "x-custom-header", "Authorization", "x-filename")
                .WithExposedHeaders("Content-Disposition")
                .AllowAnyMethod()
                .WithOrigins(
                    "http://localhost:8080"
                    // TODO: add deployed web app URL here as well
                );
        });
    }
    else
    {
        app.UseCors(options =>
        {
            options.WithHeaders("accept", "content-type", "origin", "x-custom-header", "Authorization", "x-filename")
                .WithExposedHeaders("Content-Disposition")
                .AllowAnyMethod();
                // TODO: add deployed api URL here as well
        });

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Share Web API");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Recipe Share Web API v2");
    });
    app.MapSwagger().RequireAuthorization();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseExceptionHandler(options => options.UseMiddleware<ExceptionMiddleware>());
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllerRoute("default", "api/v{version}/{controller}/{action}")
        .RequireAuthorization()
        .DisableRateLimiting();

    await app.RunAsync();

}
finally
{
    // TODO: do something
}

static string GetUserEndPoint(HttpContext context) =>
    $"User {context.User.Identity?.Name ?? "Anonymous"} endpoint:{context.Request.Path}"
    + $" {context.Connection.RemoteIpAddress}";

public abstract partial class Program { }