using RecipeShareLibrary.Model.MasterData;

namespace RecipeShareLibrary.Validator.MasterData;

public interface IIngredientValidator
{
    void ValidateSave(IIngredient save);
}