using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Manager.MasterData;

public interface IIngredientManager
{
    Task<IIngredient> GetAsync(long id);
    Task<IEnumerable<IIngredient>> GetListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<IIngredient>> GetListAsync(long[] ids, CancellationToken cancellationToken = default);
    Task<IIngredient> SaveAsync(IUser user, IIngredient save);
}