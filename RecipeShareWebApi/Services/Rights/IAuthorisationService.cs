using RecipeShareLibrary.Model.Rights;

namespace RecipeShareWebApi.Services.Rights;

public interface IAuthorisationService
{
    Task<IUserToken> LogInAsync(string email, string password);
    Task LogoutAsync();
    Task<IUserToken> GetSessionAsync(CancellationToken cancellationToken);
}