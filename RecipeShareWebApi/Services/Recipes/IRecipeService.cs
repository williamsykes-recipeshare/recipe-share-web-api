using RecipeShareLibrary.Model.Recipes;

namespace RecipeShareWebApi.Services.Recipes;

public interface IRecipeService
{
    Task<IRecipe> GetAsync(long id);
    Task<IEnumerable<IRecipe>> GetListAsync(CancellationToken cancellationToken);
    Task<IEnumerable<IRecipe>> GetFilteredListAsync(RecipeFilters filters, CancellationToken cancellationToken);
    Task<IRecipe> SaveAsync(IRecipe save);
}