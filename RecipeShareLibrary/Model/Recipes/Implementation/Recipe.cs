using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareLibrary.Model.Recipes.Implementation;

public class Recipe : BaseModel, IRecipe
{
    public Guid Guid { get; set; }
    public required string Name { get; set; }
    public int CookingTimeMinutes { get; set; }

    public ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
    public ICollection<RecipeDietaryTag>? RecipeDietaryTags { get; set; }
    public ICollection<Step>? Steps { get; set; }
}