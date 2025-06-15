using RecipeShareLibrary.Model.Rights;

namespace RecipeShareWebApi.Services.Rights;

public interface IRightService
{
    Task<IEnumerable<IRight>> GetListAsync(IUser user, CancellationToken cancellationToken);
    Task<IEnumerable<IRight>> GetAllAsync(CancellationToken cancellationToken);
}