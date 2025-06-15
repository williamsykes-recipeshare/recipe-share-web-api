using RecipeShareLibrary.DBContext.Implementation;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.DBContext;

public interface IRecipeShareDbContextFactory : IDbContextFactory<RecipeShareDbContext>
{
    DbContextOptions<RecipeShareDbContext> options { get; }
}