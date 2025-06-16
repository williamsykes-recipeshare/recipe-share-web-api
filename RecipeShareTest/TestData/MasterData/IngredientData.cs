using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareTest.TestData.MasterData;

public static class IngredientData
{
    public static readonly Guid ExistingGuid = Guid.NewGuid();

    public static IIngredient CreateExisting() => new Ingredient()
    {
        Id = 1,
        Guid = ExistingGuid,
        Name = "Chicken",
        CreatedById = 1,
        CreatedByName = "Tester",
        UpdatedById = 1,
        UpdatedByName = "Tester",
    };

    public static IIngredient CreateNew() => new Ingredient()
    {
        Id = 2,
        Guid = Guid.NewGuid(),
        Name = "Beef",
        CreatedById = 1,
        CreatedByName = "Tester",
        UpdatedById = 1,
        UpdatedByName = "Tester",
    };

    public static async Task SeedData(RecipeShareDbContext dbContext)
    {
        #region Declaration

        var data = new List<IIngredient>
        {
            CreateExisting(),
            CreateNew(),
            new Ingredient()
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Shrimp",
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
                IsActive = false,
            },
        };

        #endregion

        await dbContext.Database.EnsureCreatedAsync();
        dbContext.AddRange(data);
        await dbContext.SaveChangesAsync();
    }
}