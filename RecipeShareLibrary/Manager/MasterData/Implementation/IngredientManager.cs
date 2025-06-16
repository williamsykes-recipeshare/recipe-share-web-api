using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Validator.MasterData;

namespace RecipeShareLibrary.Manager.MasterData.Implementation;

public class IngredientManager(
    IRecipeShareDbContextFactory dbContextFactory,
    IIngredientValidator ingredientValidator
) : IIngredientManager
{

    #region Get Methods

    /// <summary>
    /// Returns a single ingredient based on the specified ID.
    /// An exception is thrown if no record matching the ID was found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IIngredient> GetAsync(long id)
    {
        var resultList = await GetListAsync(new[] { id });

        return resultList.Single();
    }

    /// <summary>
    /// Returns all ingredients
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IIngredient>> GetListAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Ingredients.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all ingredients matching the specified array of IDs.
    /// An exception is thrown if a corresponding record was not found for any of the specified IDs
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<IEnumerable<IIngredient>> GetListAsync(long[] ids, CancellationToken cancellationToken = default)
    {
        if (!ids.Any())
            throw new NotFoundException("Invalid ingredient.");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var resultQuery = dbContext.Ingredients
            .Where(x => ids.Contains(x.Id));

        if (ids.Distinct().Count() != await resultQuery.CountAsync(cancellationToken))
            throw new NotFoundException("Invalid ingredient.");

        return await resultQuery.ToListAsync(cancellationToken);
    }

    #endregion

    #region Insert/Update Methods

    /// <summary>
    /// Creates or Inserts a new ingredient.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="save"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public async Task<IIngredient> SaveAsync(IUser user, IIngredient save)
    {
        #region Validation

        ingredientValidator.ValidateSave(save);

        #endregion

        IIngredient result;

        if (save.Id == 0)
        {
            // Check that the Guid is valid.
            var existingGuid = await GetAsync(save.Guid);
            if (existingGuid != null)
                throw new BadRequestException("Duplicate guid.");

            result = new Ingredient
            {
                Guid = save.Guid,
                Name = save.Name,
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

        // Check that the name is not already being used by another record.
        var existingName = await GetAsync(result.Name);
        if (existingName != null && existingName.Id != save.Id)
            throw new BadRequestException("Duplicate name.");

        result.Name = save.Name;
        result.IsActive = save.IsActive ?? true;
        result.UpdatedById = user.Id;
        result.UpdatedByName = user.Name;

        await dbContext.SaveChangesAsync();

        return result;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Returns a single ingredient based on the specified GUID.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    private async Task<IIngredient?> GetAsync(Guid guid)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Ingredients
            .SingleOrDefaultAsync(x => x.Guid == guid);
    }

    /// <summary>
    /// Returns a single ingredient based on the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private async Task<IIngredient?> GetAsync(string name)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Ingredients
            .SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }

    #endregion
}