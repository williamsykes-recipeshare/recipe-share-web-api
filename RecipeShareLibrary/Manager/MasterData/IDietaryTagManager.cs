using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Manager.MasterData;

public interface IDietaryTagManager
{
    Task<IDietaryTag> GetAsync(long id);
    Task<IEnumerable<IDietaryTag>> GetListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<IDietaryTag>> GetListAsync(long[] ids, CancellationToken cancellationToken = default);
    Task<IDietaryTag> SaveAsync(IUser user, IDietaryTag save);
}