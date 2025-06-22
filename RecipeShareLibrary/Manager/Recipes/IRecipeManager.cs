using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Manager.Recipes;

public interface IRecipeManager
{
    Task<IRecipe> GetAsync(long id);
    Task<IEnumerable<IRecipe>> GetListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<IRecipe>> GetListAsync(long[] ids, CancellationToken cancellationToken = default);
    Task<IEnumerable<IRecipe>> GetFilteredListAsync(RecipeFilters filters, CancellationToken cancellationToken = default);
    Task<IRecipe> SaveAsync(IUser user, IRecipe save);
}