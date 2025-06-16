using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Manager.Rights.Implementation;
using RecipeShareLibrary.Model.Settings;
using RecipeShareLibrary.Validator.Rights.Implementation;

namespace RecipeShareTest.Helpers.Manager;

public static class RightsManagerHelper
{
    public static IUserManager CreateUserManager(Func<IRecipeShareDbContextFactory> dbFactory,
        ApplicationSettings? applicationSettings = null)
    {
        return new UserManager(
            dbFactory(),
            new UserTokenManager(dbFactory(), ApplicationSettingsHelper.GetApplicationSettings()),
            new UserValidator(applicationSettings ?? ApplicationSettingsHelper.GetApplicationSettings().Value),
            new RightManager(dbFactory())
        );
    }
}