using RecipeShareLibrary.Model.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RecipeShareTest.Helpers;

public static class ApplicationSettingsHelper
{
    public static IOptions<ApplicationSettings> GetApplicationSettings()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "UnitTesting"
        });

        builder.Configuration
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        builder.Services.AddOptions();
        builder.Services.Configure<ApplicationSettings>(
            builder.Configuration.GetSection("ApplicationSettings"));

        var app = builder.Build();
        var applicationSettingsOptions = app.Services.GetRequiredService<IOptions<ApplicationSettings>>();
        var _ = app.DisposeAsync().ConfigureAwait(false);

        return applicationSettingsOptions;
    }
}