using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Manager.Rights;

public interface IUserManager
{
    Task RegisterUserAsync(string name, string email, string password);
    Task<IUserToken> AuthenticateUserAsync(string email, string password, string userAgent, string remoteIp);
    Task<IUser?> GetAsync(string email);
    Task<IUser> GetAsync(long userId, CancellationToken cancellationToken = default);
    Task<IUser> SaveAsync(IUser user, IUser save);
    Task<IEnumerable<IUser>> GetListAsync(long[] userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<IUser>> GetListAsync(CancellationToken cancellationToken = default);
    Task<IUser> UpdateLastLoginAsync(long userId);
    Task UpdatePasswordAsync(IUser user, string password);
    Task UpdatePasswordAsync(IUser updatedBy, long userId, string password);
    Task DeleteAsync(IUser user, long id);
}