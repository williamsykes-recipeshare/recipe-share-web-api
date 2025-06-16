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

public class IngredientManagerTest : TestWithSqlite
{
    private IIngredientManager? _ingredientManager;
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
        await TestData.MasterData.IngredientData.SeedData(dbContext);

        _userManager = RightsManagerHelper.CreateUserManager(DbFactory);
        _ingredientManager = new IngredientManager(DbFactory(), new IngredientValidator());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetAsync_ValidId_ReturnIngredient(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _ingredientManager!.GetAsync(id);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IIngredient>(x => x.Id == id);

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
        Func<Task> act = async () => await _ingredientManager!.GetAsync(id);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid ingredient.");

        #endregion
    }

    [Fact]
    public async Task GetListAsync_NoArgs_ReturnIngredients()
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _ingredientManager!.GetListAsync();

        #region Assert

        result.Should().NotBeEmpty()
            .And.OnlyContain(x => x.Id > 0);

        #endregion
    }

    [Theory]
    [InlineData(new long[] { 1, 2, 3 })]
    [InlineData(new long[] { 2 })]
    public async Task GetListAsync_ValidIds_ReturnIngredients(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _ingredientManager!.GetListAsync(ids);

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
        Func<Task> act = async () => await _ingredientManager!.GetListAsync(ids);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid ingredient.");

        #endregion
    }

    [Theory]
    [ClassData(typeof(IngredientInvalidSaveArgs))]
    public async Task SaveAsync_InValidIngredient_ThrowsBadRequest(long userId, IIngredient save, string exception, Type exceptionType)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        Func<Task> act = async () => await _ingredientManager!.SaveAsync(user, save);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .Where(x => x.GetType() == exceptionType).WithMessage(exception);
    }

    [Theory]
    [ClassData(typeof(IngredientValidSaveArgs))]
    public async Task SaveAsync_ValidIngredient_ReturnIngredient(long userId, IIngredient save)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        var result = await _ingredientManager!.SaveAsync(user, save);

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


        var inserted = await _ingredientManager!.GetAsync(result.Id);

        result.Should().NotBeNull()
            .And.BeEquivalentTo(inserted, config =>
                config);
    }
}