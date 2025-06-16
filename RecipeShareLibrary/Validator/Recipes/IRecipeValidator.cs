using RecipeShareLibrary.Model.Recipes;

namespace RecipeShareLibrary.Validator.Recipes;

public interface IRecipeValidator
{
    void ValidateSave(IRecipe save);
}