using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.Manager.Rights.Implementation;

public class RightManager(IRecipeShareDbContextFactory dbContextFactory) : IRightManager
{
    public async Task<IRight> GetAsync(long id)
    {
        var resultList = await GetListAsync(new[] { id });

        return resultList.Single();
    }

    public async Task<IEnumerable<IRight>> GetListAsync(long[] ids, CancellationToken cancellationToken = default)
    {
        if (ids.Length == 0)
            throw new NotFoundException("Invalid right.");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var resultQuery = dbContext.Rights
            .Where(x => ids.Contains(x.Id));

        if (ids.Distinct().Count() != await resultQuery.CountAsync(cancellationToken))
            throw new NotFoundException("Invalid right.");

        return await resultQuery.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all rights
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<IRight>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        var result = await dbContext.Rights.ToListAsync(cancellationToken);

        return result;
    }
}