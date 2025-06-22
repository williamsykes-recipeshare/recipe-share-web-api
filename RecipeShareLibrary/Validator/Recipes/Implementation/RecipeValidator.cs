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

        if (save.CookingTimeMinutes < 0)
            throw new BadRequestException("Invalid cooking time.");

        if (save.RecipeIngredients != null && save.RecipeIngredients.Select(x => x.IngredientId).Any(x => x <= 0))
            throw new BadRequestException("Invalid ingredient ID(s).");

        if (save.RecipeIngredients != null && save.RecipeIngredients.Select(x => x.Quantity).Any(x => x < 0))
            throw new BadRequestException("Invalid ingredient quantities.");

        if (save.RecipeDietaryTags != null && save.RecipeDietaryTags.Select(x => x.DietaryTagId).Any(x => x <= 0))
            throw new BadRequestException("Invalid dietary tag ID(s).");

        if (save.Steps != null)
        {
            foreach (var step in save.Steps)
            {
                if (string.IsNullOrWhiteSpace(step.Name))
                    throw new BadRequestException("Invalid step name.");

                if (step.Index < 0)
                    throw new BadRequestException("Invalid step index.");
            }

            var duplicateIndexes = save.Steps
                    .GroupBy(x => x.Index)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

            if (duplicateIndexes.Count > 0)
            {
                throw new BadRequestException("Duplicate step indexes");
            }
        }
    }
}