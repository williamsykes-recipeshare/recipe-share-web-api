using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Manager.Rights;

public interface IUserTokenManager
{
    Task<IUserToken> GenerateTokenAsync(IUser user, string userAgent, string remoteIp);
    Task RemoveAsync(Guid guid);
    Task<IUserToken> GetSessionAsync(Guid tokenGuid, CancellationToken cancellationToken = default);
}