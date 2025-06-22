using RecipeShareLibrary.Manager.Recipes;
using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomExceptions;

namespace RecipeShareWebApi.Services.Recipes.Implementation;

public class RecipeService(IRecipeManager recipeManager, IHttpContextAccessor httpContextAccessor) : IRecipeService
{
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;

    public async Task<IRecipe> GetAsync(long id)
    {
        return await recipeManager.GetAsync(id);
    }

    public async Task<IEnumerable<IRecipe>> GetListAsync(CancellationToken cancellationToken)
    {
        return await recipeManager.GetListAsync(cancellationToken);
    }

    public async Task<IEnumerable<IRecipe>> GetFilteredListAsync(RecipeFilters filters, CancellationToken cancellationToken)
    {
        return await recipeManager.GetFilteredListAsync(filters, cancellationToken);
    }

    public async Task<IRecipe> SaveAsync(IRecipe save)
    {
        if (_httpContext?.Items["User"] is not IUser user) throw new ForbiddenException("");

        return await recipeManager.SaveAsync(user, save);
    }
}