using RecipeShareLibrary.Model.Settings;
using Microsoft.Extensions.Options;

namespace RecipeShareBenchmark.Helpers;

public class BenchmarkWithSettings
{
    protected readonly IOptions<ApplicationSettings> ApplicationSettingsOptions;

    protected BenchmarkWithSettings()
    {
        ApplicationSettingsOptions = BenchmarkApplicationSettingsHelper.GetApplicationSettings();
    }
}