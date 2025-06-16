using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareLibrary.Model.Recipes.Implementation;

public class RecipeDietaryTag : BaseModel, IRecipeDietaryTag
{
    public long DietaryTagId { get; set; }
    public DietaryTag? DietaryTag { get; set; }
    public long RecipeId { get; set; }
}