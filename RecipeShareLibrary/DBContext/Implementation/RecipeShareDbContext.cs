using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareLibrary.Model.Recipes.Implementation;
using RecipeShareLibrary.Model.Rights.Implementation;
using RecipeShareLibrary.ModelBuilders;

namespace RecipeShareLibrary.DBContext.Implementation;

public class RecipeShareDbContext(DbContextOptions<RecipeShareDbContext> options) : DbContext(options)
{
    #region Rights

    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<UserRight> UserRights => Set<UserRight>();
    public virtual DbSet<UserToken> UserTokens => Set<UserToken>();
    public virtual DbSet<Right> Rights => Set<Right>();
    public virtual DbSet<UserVerifyToken> UserVerifyTokens => Set<UserVerifyToken>();

    #endregion

    #region Master Data

    public virtual DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public virtual DbSet<Step> Steps => Set<Step>();

    #endregion

    #region Recipes

    public virtual DbSet<Recipe> Recipes => Set<Recipe>();
    public virtual DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    #endregion


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.BuildRights();
        modelBuilder.BuildMasterData();
        modelBuilder.BuildRecipes();

        if (Database.ProviderName != "Microsoft.EntityFrameworkCore.Sqlite") return;

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
            var nullableProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal?));

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
            }

            foreach (var property in nullableProperties)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double?>();
            }
        }
    }
}