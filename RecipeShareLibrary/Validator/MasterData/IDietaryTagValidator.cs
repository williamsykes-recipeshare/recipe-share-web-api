using RecipeShareLibrary.Model.MasterData;

namespace RecipeShareLibrary.Validator.MasterData;

public interface IDietaryTagValidator
{
    void ValidateSave(IDietaryTag save);
}