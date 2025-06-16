using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareLibrary.Model.Recipes.Implementation;

public class RecipeIngredient : BaseModel, IRecipeIngredient
{
    public long IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
    public long RecipeId { get; set; }

    public int Quantity { get; set; }
}