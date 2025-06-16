using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareLibrary.Model.Recipes.Implementation;

namespace RecipeShareLibrary.Model.Recipes;

public interface IRecipe : IBaseModel
{
    Guid Guid { get; set; }
    string Name { get; set; }
    int CookingTimeMinutes { get; set; }

    ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
    ICollection<RecipeDietaryTag>? RecipeDietaryTags { get; set; }
    ICollection<Step>? Steps { get; set; }
}