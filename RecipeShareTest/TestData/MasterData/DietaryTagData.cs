using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareTest.TestData.MasterData;

public static class DietaryTagData
{
    public static readonly Guid ExistingGuid = Guid.NewGuid();

    public static IDietaryTag CreateExisting() => new DietaryTag()
    {
        Id = 1,
        Guid = ExistingGuid,
        Name = "Vegan",
        CreatedById = 1,
        CreatedByName = "Tester",
        UpdatedById = 1,
        UpdatedByName = "Tester",
    };

    public static IDietaryTag CreateNew() => new DietaryTag()
    {
        Id = 2,
        Guid = Guid.NewGuid(),
        Name = "Organic",
        CreatedById = 1,
        CreatedByName = "Tester",
        UpdatedById = 1,
        UpdatedByName = "Tester",
    };

    public static async Task SeedData(RecipeShareDbContext dbContext)
    {
        #region Declaration

        var data = new List<IDietaryTag>
        {
            CreateExisting(),
            CreateNew(),
            new DietaryTag()
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Nut-free",
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