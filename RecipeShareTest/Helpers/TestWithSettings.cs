using RecipeShareLibrary.Model.Settings;
using Microsoft.Extensions.Options;

namespace RecipeShareTest.Helpers;

public class TestWithSettings : TestWithSqlite
{
    protected readonly IOptions<ApplicationSettings> ApplicationSettingsOptions;

    protected TestWithSettings()
    {
        ApplicationSettingsOptions = ApplicationSettingsHelper.GetApplicationSettings();
    }
}