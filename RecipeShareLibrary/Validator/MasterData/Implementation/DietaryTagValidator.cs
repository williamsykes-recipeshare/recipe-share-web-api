using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.MasterData;

namespace RecipeShareLibrary.Validator.MasterData.Implementation;

public class DietaryTagValidator : IDietaryTagValidator
{
    public void ValidateSave(IDietaryTag save)
    {
        if (save.Id < 0)
            throw new NotFoundException("Invalid dietary tag.");

        if (save.Guid == new Guid())
            throw new BadRequestException("Invalid guid.");

        if (string.IsNullOrWhiteSpace(save.Name))
            throw new BadRequestException("Invalid name.");
    }
}