using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.DBContext.Implementation;

public class RecipeShareDbContextFactory(DbContextOptions<RecipeShareDbContext> _options = null)
    : IRecipeShareDbContextFactory
{
    public DbContextOptions<RecipeShareDbContext> options => _options;

    public RecipeShareDbContext CreateDbContext()
    {
        return new RecipeShareDbContext(options);
    }

    public async Task<RecipeShareDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return new RecipeShareDbContext(options);
    }
}