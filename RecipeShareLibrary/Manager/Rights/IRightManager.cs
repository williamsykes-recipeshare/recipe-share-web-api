using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Manager.Rights;

public interface IRightManager
{
    Task<IRight> GetAsync(long id);
    Task<IEnumerable<IRight>> GetListAsync(long[] ids, CancellationToken cancellationToken = default);
    Task<IEnumerable<IRight>> GetAllAsync(CancellationToken cancellationToken = default);
}