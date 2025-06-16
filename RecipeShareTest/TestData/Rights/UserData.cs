using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Rights.Implementation;

namespace RecipeShareTest.TestData.Rights;

public static class UserData
{
    public static readonly Guid ExistingGuid = Guid.NewGuid();

    public static async Task SeedData(RecipeShareDbContext dbContext)
    {
        #region Declaration

        PasswordHelper.HashPassword("Password123", out byte[] password);

        var data = new List<User>
        {
            new ()
            {
                Id = 1,
                Guid = Guid.NewGuid(),
                Name = "Test User 1",
                UserPassword = new UserPassword()
                {
                    Password = password,
                },
                Email = "user1@mail.com",
                IsActive = true,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
            new User
            {
                Id = 2,
                Guid = ExistingGuid,
                Name = "Test User 2",
                UserPassword = new UserPassword()
                {
                    Password = password,
                },
                Email = "user2@mail.com",
                IsActive = true,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
            new User
            {
                Id = 3,
                Guid = Guid.NewGuid(),
                Name = "Inactive User",
                UserPassword = new UserPassword()
                {
                    Password = password,
                },
                Email = "user3@mail.com",
                IsActive = false,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
            new User
            {
                Id = 4,
                Guid = Guid.NewGuid(),
                Name = "Updatable user",
                UserPassword = new UserPassword()
                {
                    Password = password,
                },
                Email = "user4@mail.com",
                IsActive = true,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
            new User
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                Name = "Test User 5",
                UserPassword = new UserPassword()
                {
                    Password = password,
                },
                Email = "user5@mail.com",
                IsActive = true,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
            new User
            {
                Id = 5,
                Guid = Guid.NewGuid(),
                Name = "Test User 6",
                UserPassword = new UserPassword()
                {
                    Password = password,
                },
                Email = "user6@mail.com",
                IsActive = true,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
        };

        #endregion

        await dbContext.Database.EnsureCreatedAsync();
        dbContext.AddRange(data);
        await dbContext.SaveChangesAsync();
    }

    public static IUser CreateNewSave()
    {
        return new User()
        {
            Email = "new@email.com",
            Guid = Guid.NewGuid(),
            Name = "New User",
            UserRights = new List<UserRight>()
            {
                new()
                {
                    RightId = RightConstants.Rights,
                    CreatedById = 1,
                    CreatedByName = "Test User 1",
                    UpdatedById = 1,
                    UpdatedByName = "Test User 1",
                },
                new()
                {
                    RightId = RightConstants.UserRights,
                    CreatedById = 1,
                    CreatedByName = "Test User 1",
                    UpdatedById = 1,
                    UpdatedByName = "Test User 1",
                },
            },
            UserPassword = new UserPassword()
            {
                Password = "Password1234!?"u8.ToArray(),
            },
            CreatedById = 1,
            CreatedByName = "Test User 1",
            UpdatedById = 1,
            UpdatedByName = "Test User 1",
        };
    }

    public static IUser CreateExistingSave()
    {
        var user = CreateNewSave();
        user.Id = 2;
        user.Guid = ExistingGuid;
        user.Email = "user2@mail.com";

        user.UserRights = new List<UserRight>()
        {
            new ()
            {
                RightId = 1,
                CreatedById = 1,
                CreatedByName = "Test User 1",
                UpdatedById = 1,
                UpdatedByName = "Test User 1",
            },
        };

        return user;
    }
}