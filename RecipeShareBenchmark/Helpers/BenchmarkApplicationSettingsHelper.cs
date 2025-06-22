using RecipeShareLibrary.Model.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace RecipeShareBenchmark.Helpers;

public static class BenchmarkApplicationSettingsHelper
{
    public static IOptions<ApplicationSettings> GetApplicationSettings()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        builder.Services.AddOptions();
        builder.Services.Configure<ApplicationSettings>(
            builder.Configuration.GetSection("ApplicationSettings"));

        var app = builder.Build();
        var applicationSettingsOptions = app.Services.GetRequiredService<IOptions<ApplicationSettings>>();
        var _ = app.DisposeAsync().ConfigureAwait(false);

        return applicationSettingsOptions;
    }
}
