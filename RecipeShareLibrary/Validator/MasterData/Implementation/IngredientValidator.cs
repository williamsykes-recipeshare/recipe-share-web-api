using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.MasterData;

namespace RecipeShareLibrary.Validator.MasterData.Implementation;

public class IngredientValidator : IIngredientValidator
{
    public void ValidateSave(IIngredient save)
    {
        if (save.Id < 0)
            throw new NotFoundException("Invalid ingredient.");

        if (save.Guid == new Guid())
            throw new BadRequestException("Invalid guid.");

        if (string.IsNullOrWhiteSpace(save.Name))
            throw new BadRequestException("Invalid name.");
    }
}