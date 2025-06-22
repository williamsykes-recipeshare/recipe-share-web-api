using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Model.Recipes.Implementation;

namespace RecipeShareTest.TestData.Recipes;

public static class RecipeData
{
    public static readonly Guid ExistingGuid = Guid.NewGuid();

    public static IRecipe CreateExisting() => new Recipe()
    {
        Id = 1,
        Guid = ExistingGuid,
        Name = "Chicken",
        CookingTimeMinutes = 45,
        CreatedById = 1,
        CreatedByName = "Tester",
        UpdatedById = 1,
        UpdatedByName = "Tester",
        RecipeIngredients = new List<RecipeIngredient>
        {
            new RecipeIngredient { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = 100, CreatedById = 1, CreatedByName = "Tester", UpdatedById = 1, UpdatedByName = "Tester", IsActive = true },
            new RecipeIngredient { Id = 2, RecipeId = 1, IngredientId = 2, Quantity = 200, CreatedById = 1, CreatedByName = "Tester", UpdatedById = 1, UpdatedByName = "Tester", IsActive = true },
        },
        RecipeDietaryTags = new List<RecipeDietaryTag>
        {
            new RecipeDietaryTag { Id = 1, RecipeId = 1, DietaryTagId = 1, CreatedById = 1, CreatedByName = "Tester", UpdatedById = 1, UpdatedByName = "Tester", IsActive = true },
            new RecipeDietaryTag { Id = 2, RecipeId = 1, DietaryTagId = 2, CreatedById = 1, CreatedByName = "Tester", UpdatedById = 1, UpdatedByName = "Tester", IsActive = true },
        },
        Steps = new List<Step>(),
    };

    public static IRecipe CreateNew() => new Recipe()
    {
        Id = 2,
        Guid = Guid.NewGuid(),
        Name = "Beef",
        CookingTimeMinutes = 60,
        CreatedById = 1,
        CreatedByName = "Tester",
        UpdatedById = 1,
        UpdatedByName = "Tester",
        RecipeIngredients = new List<RecipeIngredient>
        {
            new RecipeIngredient { Id = 3, RecipeId = 2, IngredientId = 3, Quantity = 150, CreatedById = 1, CreatedByName = "Tester", UpdatedById = 1, UpdatedByName = "Tester", IsActive = true }
        },
        RecipeDietaryTags = new List<RecipeDietaryTag>
        {
            new RecipeDietaryTag { Id = 3, RecipeId = 2, DietaryTagId = 3, CreatedById = 1, CreatedByName = "Tester", UpdatedById = 1, UpdatedByName = "Tester", IsActive = true },
        },
        Steps = new List<Step>(),
    };

    public static async Task SeedData(RecipeShareDbContext dbContext)
    {
        var data = new List<IRecipe>
        {
            CreateExisting(),
            CreateNew(),
        };

        await dbContext.Database.EnsureCreatedAsync();
        dbContext.AddRange(data);
        await dbContext.SaveChangesAsync();
    }
}