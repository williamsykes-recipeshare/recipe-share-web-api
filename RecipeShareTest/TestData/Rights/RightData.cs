using System.Reflection;
using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Rights.Implementation;

namespace RecipeShareTest.TestData.Rights;

public static class RightData
{
    public static async Task SeedData(RecipeShareDbContext dbContext)
    {
        var rights = typeof(RightConstants).GetMembers(BindingFlags.Public|BindingFlags.Static);

        #region Declaration

        var data = new List<IRight>
        {
            new Right()
            {
                Id = RightConstants.Rights,
                Code = "Rights",
                Name = "Rights",
                Type = RightsEnum.EnumRightType.Right,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
            new Right()
            {
                Id = RightConstants.UserRights,
                ParentId = RightConstants.Rights,
                Code = "UserRights",
                Name = "Users",
                Type = RightsEnum.EnumRightType.Right,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
            new Right()
            {
                Id = RightConstants.MasterData,
                Code = "MasterData",
                Name = "MasterData",
                Type = RightsEnum.EnumRightType.MasterData,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
        };

        #endregion

        if (data.Count != rights.Length)
            throw new Exception("Not all rights declared!");

        await dbContext.Database.EnsureCreatedAsync();
        dbContext.AddRange(data);
        await dbContext.SaveChangesAsync();
    }
}