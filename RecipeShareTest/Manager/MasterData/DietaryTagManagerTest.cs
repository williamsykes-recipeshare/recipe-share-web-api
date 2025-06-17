using FluentAssertions;
using RecipeShareLibrary.Manager.MasterData;
using RecipeShareLibrary.Manager.MasterData.Implementation;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Validator.MasterData.Implementation;
using RecipeShareTest.Helpers;
using RecipeShareTest.Helpers.Manager;
using RecipeShareTest.TestData.Arguments.MasterData;
using Xunit;

namespace RecipeShareTest.Manager.MasterData;

public class DietaryTagManagerTest : TestWithSqlite
{
    private IDietaryTagManager? _dietaryTagManager;
    private IUserManager? _userManager;

    [Fact]
    public async Task DatabaseIsAvailableAndCanBeConnectedTo()
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        Assert.True(await dbContext.Database.CanConnectAsync());
    }

    private async Task SetUp()
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        await TestData.Rights.RightData.SeedData(dbContext);
        await TestData.Rights.UserData.SeedData(dbContext);
        await TestData.MasterData.DietaryTagData.SeedData(dbContext);

        _userManager = RightsManagerHelper.CreateUserManager(DbFactory);
        _dietaryTagManager = new DietaryTagManager(DbFactory(), new DietaryTagValidator());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetAsync_ValidId_ReturnDietaryTag(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _dietaryTagManager!.GetAsync(id);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IDietaryTag>(x => x.Id == id);

        #endregion
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(long.MaxValue)]
    public async Task GetAsync_InValidId_ThrowsNotFoundException(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _dietaryTagManager!.GetAsync(id);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid dietary tag.");

        #endregion
    }

    [Fact]
    public async Task GetListAsync_NoArgs_ReturnDietaryTags()
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _dietaryTagManager!.GetListAsync();

        #region Assert

        result.Should().NotBeEmpty()
            .And.OnlyContain(x => x.Id > 0);

        #endregion
    }

    [Theory]
    [InlineData(new long[] { 1, 2, 3 })]
    [InlineData(new long[] { 2 })]
    public async Task GetListAsync_ValidIds_ReturnDietaryTags(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _dietaryTagManager!.GetListAsync(ids);

        #region Assert

        result.Should().NotBeEmpty()
            .And.HaveCount(ids.Length)
            .And.OnlyContain(x => ids.Contains(x.Id));

        #endregion
    }

    [Theory]
    [InlineData(new [] { long.MaxValue, 2 })]
    [InlineData(new long[] {  })]
    public async Task GetListAsync_InValidIds_ThrowsNotFoundException(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _dietaryTagManager!.GetListAsync(ids);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid dietary tag.");

        #endregion
    }

    [Theory]
    [ClassData(typeof(DietaryTagInvalidSaveArgs))]
    public async Task SaveAsync_InValidDietaryTag_ThrowsBadRequest(long userId, IDietaryTag save, string exception, Type exceptionType)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        Func<Task> act = async () => await _dietaryTagManager!.SaveAsync(user, save);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .Where(x => x.GetType() == exceptionType).WithMessage(exception);
    }

    [Theory]
    [ClassData(typeof(DietaryTagValidSaveArgs))]
    public async Task SaveAsync_ValidDietaryTag_ReturnDietaryTag(long userId, IDietaryTag save)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        var result = await _dietaryTagManager!.SaveAsync(user, save);

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.BeValidAudit(save, user);

        result.Should().NotBeNull()
            .And.BeEquivalentTo(save, config =>
                config
                    .Excluding(x => x.Id)
                    .Excluding(x => x.CreatedById)
                    .Excluding(x => x.CreatedByName)
                    .Excluding(x => x.CreatedOn)
                    .Excluding(x => x.UpdatedById)
                    .Excluding(x => x.UpdatedByName)
                    .Excluding(x => x.UpdatedOn)
                    .Excluding(x => x.IsActive));


        var inserted = await _dietaryTagManager!.GetAsync(result.Id);

        result.Should().NotBeNull()
            .And.BeEquivalentTo(inserted, config =>
                config);
    }
}