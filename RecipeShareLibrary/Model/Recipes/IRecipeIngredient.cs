using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareLibrary.Model.Recipes;

public interface IRecipeIngredient : IBaseModel
{
    long IngredientId { get; set; }
    Ingredient? Ingredient { get; set; }
    long RecipeId { get; set; }

    string Quantity { get; set; }
}