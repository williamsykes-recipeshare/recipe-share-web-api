using RecipeShareLibrary.Manager.MasterData;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomExceptions;

namespace RecipeShareWebApi.Services.MasterData.Implementation;

public class IngredientService(IIngredientManager ingredientManager, IHttpContextAccessor httpContextAccessor) : IIngredientService
{
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;

    public async Task<IIngredient> GetAsync(long id)
    {
        return await ingredientManager.GetAsync(id);
    }

    public async Task<IEnumerable<IIngredient>> GetListAsync(CancellationToken cancellationToken)
    {
        return await ingredientManager.GetListAsync(cancellationToken);
    }

    public async Task<IIngredient> SaveAsync(IIngredient save)
    {
        if (_httpContext?.Items["User"] is not IUser user) throw new ForbiddenException("");

        return await ingredientManager.SaveAsync(user, save);
    }
}