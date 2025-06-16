using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Rights.Implementation;

namespace RecipeShareTest.TestData.Rights;

public static class UserRightData
{
    public static async Task SeedData(RecipeShareDbContext dbContext)
    {
        #region Declaration

        var data = new List<IUserRight>
        {

            new UserRight
            {
                RightId = RightConstants.Rights,
                UserId = 1,
                CreatedById = 1,
                CreatedByName = "Test User",
                UpdatedById = 1,
                UpdatedByName = "Test User",
            },
            new UserRight
            {
                RightId = RightConstants.UserRights,
                UserId = 1,
                CreatedById = 1,
                CreatedByName = "Test User",
                UpdatedById = 1,
                UpdatedByName = "Test User",
            },
            new UserRight
            {
                RightId = RightConstants.MasterData,
                UserId = 1,
                CreatedById = 1,
                CreatedByName = "Test User",
                UpdatedById = 1,
                UpdatedByName = "Test User",
            },
            new UserRight
            {
                RightId = RightConstants.Rights,
                UserId = 5,
                CreatedById = 1,
                CreatedByName = "Test User",
                UpdatedById = 1,
                UpdatedByName = "Test User",
            },
            new UserRight
            {
                RightId = RightConstants.UserRights,
                UserId = 5,
                CreatedById = 1,
                CreatedByName = "Test User",
                UpdatedById = 1,
                UpdatedByName = "Test User",
            },
        };

        #endregion

        await dbContext.Database.EnsureCreatedAsync();
        dbContext.AddRange(data);
        await dbContext.SaveChangesAsync();
    }
}