using FluentAssertions;
using RecipeShareLibrary.Manager.Recipes;
using RecipeShareLibrary.Manager.Recipes.Implementation;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Validator.Recipes.Implementation;
using RecipeShareTest.Helpers;
using RecipeShareTest.Helpers.Manager;
using RecipeShareTest.TestData.Arguments.Recipes;
using Xunit;

namespace RecipeShareTest.Manager.MasterData;

public class RecipeManagerTest : TestWithSqlite
{
    private IRecipeManager? _recipeManager;
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
        await TestData.MasterData.IngredientData.SeedData(dbContext);
        await TestData.Recipes.RecipeData.SeedData(dbContext);

        _userManager = RightsManagerHelper.CreateUserManager(DbFactory);
        _recipeManager = new RecipeManager(DbFactory(), new RecipeValidator());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetAsync_ValidId_ReturnRecipe(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _recipeManager!.GetAsync(id);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IRecipe>(x => x.Id == id);

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
        Func<Task> act = async () => await _recipeManager!.GetAsync(id);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid recipe.");

        #endregion
    }

    [Fact]
    public async Task GetListAsync_NoArgs_ReturnRecipes()
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _recipeManager!.GetListAsync();

        #region Assert

        result.Should().NotBeEmpty()
            .And.OnlyContain(x => x.Id > 0);

        #endregion
    }

    [Theory]
    [InlineData(new long[] { 1, 2 })]
    [InlineData(new long[] { 2 })]
    public async Task GetListAsync_ValidIds_ReturnRecipes(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _recipeManager!.GetListAsync(ids);

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
        Func<Task> act = async () => await _recipeManager!.GetListAsync(ids);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid recipe.");

        #endregion
    }

    [Fact]
    public async Task GetFilteredListAsync_IncorrectFilters_ReturnRecipes()
    {
        #region Arrange

        await SetUp();

        var filters = new RecipeFilters
        {
            Name = "chicken", // We have "Chicken" in our test data so this is a case test as well
            IngredientIds = new List<long> { 0 },
            DietaryTagIds = new List<long> { 100 }
        };

        #endregion

        // Act
        var result = await _recipeManager!.GetFilteredListAsync(filters);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFilteredListAsync_CorrectFilters_ReturnRecipes()
    {
        #region Arrange

        await SetUp();

        var filters = new RecipeFilters
        {
            Name = "chicken", // We have "Chicken" in our test data so this is a case test as well
            IngredientIds = new List<long> { 1 },
            DietaryTagIds = new List<long> { 2 }
        };

        #endregion

        // Act
        var result = await _recipeManager!.GetFilteredListAsync(filters);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().OnlyContain(recipe =>
            recipe.Name.Contains(filters.Name, StringComparison.OrdinalIgnoreCase)
            && recipe.RecipeIngredients!.Any(x => filters.IngredientIds.Contains(x.IngredientId))
            && recipe.RecipeDietaryTags!.Any(x => filters.DietaryTagIds.Contains(x.DietaryTagId))
        );
    }

    [Theory]
    [ClassData(typeof(RecipeInvalidSaveArgs))]
    public async Task SaveAsync_InValidRecipe_ThrowsBadRequest(long userId, IRecipe save, string exception, Type exceptionType)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        Func<Task> act = async () => await _recipeManager!.SaveAsync(user, save);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .Where(x => x.GetType() == exceptionType).WithMessage(exception);
    }

    [Theory]
    [ClassData(typeof(RecipeValidSaveArgs))]
    public async Task SaveAsync_ValidRecipe_ReturnRecipe(long userId, IRecipe save)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        var result = await _recipeManager!.SaveAsync(user, save);

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.BeValidAudit(save, user);

        result.Should().NotBeNull()
            .And.BeEquivalentTo(save, config =>
                config
                    .Excluding(x => x.Id)
                    .Excluding(x => x.RecipeIngredients)
                    .Excluding(x => x.RecipeDietaryTags)
                    .Excluding(x => x.Steps)
                    .Excluding(x => x.CreatedById)
                    .Excluding(x => x.CreatedByName)
                    .Excluding(x => x.CreatedOn)
                    .Excluding(x => x.UpdatedById)
                    .Excluding(x => x.UpdatedByName)
                    .Excluding(x => x.UpdatedOn)
                    .Excluding(x => x.IsActive));

        result.RecipeIngredients.Should().NotBeNull();
        result.RecipeIngredients.Should().HaveCount(save.RecipeIngredients!.Count);
        result.RecipeIngredients!.Where(x => x.IsActive == true).Should()
            .OnlyContain(x => save.RecipeIngredients.Any(y => x.IngredientId == y.IngredientId && x.Quantity == y.Quantity));

        result.RecipeDietaryTags.Should().NotBeNull();
        result.RecipeDietaryTags.Should().HaveCount(save.RecipeDietaryTags!.Count);
        result.RecipeDietaryTags!.Where(x => x.IsActive == true).Should()
            .OnlyContain(x => save.RecipeDietaryTags.Any(y => x.DietaryTagId == y.DietaryTagId));

        result.Steps.Should().NotBeNull();
        foreach (var step in result.Steps!)
        {
            var expectedStep = save.Steps!.SingleOrDefault(s => s.Guid == step.Guid);
            expectedStep.Should().NotBeNull();
            step.Name.Should().Be(expectedStep!.Name);
            step.Index.Should().Be(expectedStep.Index);
            step.IsActive.Should().Be(expectedStep.IsActive);
        }

        var inserted = await _recipeManager!.GetAsync(result.Id);

        result.Should().NotBeNull()
            .And.BeEquivalentTo(inserted, config =>
                config);
    }
}