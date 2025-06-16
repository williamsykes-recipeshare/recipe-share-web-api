using FluentAssertions;
using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareTest.Helpers;
using Xunit;

namespace RecipeShareTest.TestData;

public class SeedData : TestWithSqlite
{
    [Fact]
    public async Task DatabaseIsAvailableAndCanBeConnectedTo()
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        Assert.True(await dbContext.Database.CanConnectAsync());
    }

    /// <summary>
    /// Ensures no breaking changes are done to data seeding for tests, as this could result in tests failing
    /// despite system under test functioning correctly. While this may seem like "test code testing test code",
    /// it's more of a sanity check than anything else.
    /// </summary>
    [Fact]
    public async Task All_Valid_NoExceptionThrown()
    {
        // Act
        Func<Task> act = async () =>
        {
            try
            {
                await using var dbContext = await DbFactory().CreateDbContextAsync();
                await All(dbContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;

                throw;
            }
        };

        // Assert
        await act.Should().NotThrowAsync();
    }

    public static async Task All(RecipeShareDbContext dbContext)
    {
        await AllRights(dbContext);
        await AllMasterData(dbContext);
    }

    private static async Task AllRights(RecipeShareDbContext dbContext)
    {
        await Rights.RightData.SeedData(dbContext);
        await Rights.UserData.SeedData(dbContext);
        await Rights.UserRightData.SeedData(dbContext);
    }

    private static async Task AllMasterData(RecipeShareDbContext dbContext)
    {
        await MasterData.IngredientData.SeedData(dbContext);
    }
}