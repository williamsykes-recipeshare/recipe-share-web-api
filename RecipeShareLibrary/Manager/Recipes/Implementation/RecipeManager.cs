using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Model.Recipes.Implementation;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Validator.Recipes;

namespace RecipeShareLibrary.Manager.Recipes.Implementation;

public class RecipeManager(
    IRecipeShareDbContextFactory dbContextFactory,
    IRecipeValidator recipeValidator
) : IRecipeManager
{

    #region Get Methods

    private static IQueryable<Recipe> GetQuery(RecipeShareDbContext dbContext)
    {
        return dbContext.Recipes
            .Include(x => x.RecipeIngredients!)
                .ThenInclude(x => x.Ingredient)
            .Include(x => x.RecipeDietaryTags!)
                .ThenInclude(x => x.DietaryTag)
            .Include(x => x.Steps!)
            .OrderByDescending(x => x.CreatedOn);
    }

    /// <summary>
    /// Returns a single recipe based on the specified ID.
    /// An exception is thrown if no record matching the ID was found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IRecipe> GetAsync(long id)
    {
        var resultList = await GetListAsync(new[] { id });

        return resultList.Single();
    }

    /// <summary>
    /// Returns all recipes
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IRecipe>> GetListAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await GetQuery(dbContext).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all recipes matching the specified array of IDs.
    /// An exception is thrown if a corresponding record was not found for any of the specified IDs
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<IEnumerable<IRecipe>> GetListAsync(long[] ids, CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            throw new NotFoundException("Invalid recipe.");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var resultQuery = GetQuery(dbContext).Where(x => ids.Contains(x.Id));

        if (ids.Distinct().Count() != await resultQuery.CountAsync(cancellationToken))
            throw new NotFoundException("Invalid recipe.");

        return await resultQuery.ToListAsync(cancellationToken);
    }

    #endregion

    #region Insert/Update Methods

    /// <summary>
    /// Creates or Inserts a new recipe.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="save"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public async Task<IRecipe> SaveAsync(IUser user, IRecipe save)
    {
        #region Validation

        recipeValidator.ValidateSave(save);

        #endregion

        IRecipe result;

        if (save.Id == 0)
        {
            // Check that the Guid is valid.
            var existingGuid = await GetAsync(save.Guid);
            if (existingGuid != null)
                throw new BadRequestException("Duplicate guid.");

            result = new Recipe
            {
                Guid = save.Guid,
                Name = save.Name,
                RecipeDietaryTags = [],
                RecipeIngredients = [],
                Steps = [],
                CreatedById = user.Id,
                CreatedByName = user.Name,
                UpdatedById = user.Id,
                UpdatedByName = user.Name,
            };
        }
        else
        {
            result = await GetAsync(save.Id);
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        dbContext.Attach(result);

        result.Name = save.Name;
        result.CookingTimeMinutes = save.CookingTimeMinutes;
        result.IsActive = save.IsActive ?? true;

        #region Ingredients

        if (result.RecipeIngredients != null && save.RecipeIngredients != null)
        {
            foreach (var resultRecipeIngredient in result.RecipeIngredients)
            {
                resultRecipeIngredient.IsActive = false;
                resultRecipeIngredient.UpdatedById = user.Id;
                resultRecipeIngredient.UpdatedByName = user.Name;
            }

            foreach (var saveRecipeIngredient in save.RecipeIngredients)
            {
                var recipeIngredient = result.RecipeIngredients.SingleOrDefault(x => x.IngredientId == saveRecipeIngredient.IngredientId);

                if (recipeIngredient == null)
                {
                    recipeIngredient = new RecipeIngredient
                    {
                        IngredientId = saveRecipeIngredient.IngredientId,
                        CreatedById = user.Id,
                        CreatedByName = user.Name,
                        UpdatedById = user.Id,
                        UpdatedByName = user.Name,
                    };

                    result.RecipeIngredients.Add(recipeIngredient);
                }

                recipeIngredient.IsActive = true;
                recipeIngredient.Quantity = saveRecipeIngredient.Quantity;
                recipeIngredient.UpdatedById = user.Id;
                recipeIngredient.UpdatedByName = user.Name;
            }
        }

        #endregion

        #region Dietary Tags

        if (result.RecipeDietaryTags != null && save.RecipeDietaryTags != null)
        {
            foreach (var resultRecipeDietaryTag in result.RecipeDietaryTags)
            {
                resultRecipeDietaryTag.IsActive = false;
                resultRecipeDietaryTag.UpdatedById = user.Id;
                resultRecipeDietaryTag.UpdatedByName = user.Name;
            }

            foreach (var saveRecipeDietaryTag in save.RecipeDietaryTags)
            {
                var recipeDietaryTag = result.RecipeDietaryTags.SingleOrDefault(x => x.DietaryTagId == saveRecipeDietaryTag.DietaryTagId);

                if (recipeDietaryTag == null)
                {
                    recipeDietaryTag = new RecipeDietaryTag
                    {
                        DietaryTagId = saveRecipeDietaryTag.DietaryTagId,
                        CreatedById = user.Id,
                        CreatedByName = user.Name,
                        UpdatedById = user.Id,
                        UpdatedByName = user.Name,
                    };

                    result.RecipeDietaryTags.Add(recipeDietaryTag);
                }

                recipeDietaryTag.IsActive = true;
                recipeDietaryTag.UpdatedById = user.Id;
                recipeDietaryTag.UpdatedByName = user.Name;
            }
        }

        #endregion

        #region Steps

        if (result.Steps != null && save.Steps != null)
        {
            foreach (var saveStep in save.Steps)
            {
                var step = result.Steps.FirstOrDefault(x => x.Id == saveStep.Id);

                if (step == null)
                {
                    step = new Step
                    {
                        Guid = saveStep.Guid,
                        Name = saveStep.Name,
                        CreatedById = user.Id,
                        CreatedByName = user.Name,
                        UpdatedById = user.Id,
                        UpdatedByName = user.Name,
                    };

                    result.Steps.Add(step);
                }

                step.Name = saveStep.Name;
                step.Index = saveStep.Index;
                step.IsActive = saveStep.IsActive;
                step.UpdatedById = user.Id;
                step.UpdatedByName = user.Name;
            }
        }

        #endregion

        result.UpdatedById = user.Id;
        result.UpdatedByName = user.Name;

        await dbContext.SaveChangesAsync();

        await dbContext.Entry(result)
            .Collection(x => x.RecipeIngredients!)
            .Query()
            .Include(x => x.Ingredient!)
            .LoadAsync();

        await dbContext.Entry(result)
            .Collection(x => x.Steps!)
            .LoadAsync();

        return result;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Returns a single recipe based on the specified GUID.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    private async Task<IRecipe?> GetAsync(Guid guid)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Recipes
            .SingleOrDefaultAsync(x => x.Guid == guid);
    }

    #endregion
}