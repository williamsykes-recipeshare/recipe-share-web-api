using RecipeShareLibrary.Model.Rights;

namespace RecipeShareWebApi.Services.Rights;

public interface IUserService
{
    Task<IEnumerable<IUser>> GetListAsync(CancellationToken cancellationToken);
    Task<IUser> GetAsync(long id);
    Task<IUser> SaveAsync(IUser save);
    Task UpdatePasswordAsync(string password);
    Task RegisterAsync(string name, string email, string password);
    Task DeleteAsync(long id);
}