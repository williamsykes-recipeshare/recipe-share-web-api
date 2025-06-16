using RecipeShareLibrary.Model.MasterData;

namespace RecipeShareWebApi.Services.MasterData;

public interface IIngredientService
{
    Task<IIngredient> GetAsync(long id);
    Task<IEnumerable<IIngredient>> GetListAsync(CancellationToken cancellationToken);
    Task<IIngredient> SaveAsync(IIngredient save);
}