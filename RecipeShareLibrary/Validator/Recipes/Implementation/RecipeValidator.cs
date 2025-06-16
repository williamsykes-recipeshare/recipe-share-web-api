using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Recipes;

namespace RecipeShareLibrary.Validator.Recipes.Implementation;

public class RecipeValidator : IRecipeValidator
{
    public void ValidateSave(IRecipe save)
    {
        if (save.Id < 0)
            throw new NotFoundException("Invalid recipe.");

        if (save.Guid == new Guid())
            throw new BadRequestException("Invalid guid.");

        if (string.IsNullOrWhiteSpace(save.Name))
            throw new BadRequestException("Invalid name.");

        if (save.Steps != null)
        {
            foreach (var step in save.Steps)
            {
                if (string.IsNullOrWhiteSpace(step.Name))
                    throw new BadRequestException("Invalid step name.");

                if (step.Index < 0)
                    throw new BadRequestException("Invalid step index.");
            }
        }
    }
}