using FluentAssertions;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Manager.Rights.Implementation;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights;
using RecipeShareTest.Helpers;
using Xunit;

namespace RecipeShareTest.Manager.Rights;

public class RightManagerTest : TestWithSqlite
{
    private IRightManager? _rightManager;

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

        _rightManager = new RightManager(DbFactory());
    }

    [Theory]
    [InlineData(RightConstants.Rights)]
    [InlineData(RightConstants.UserRights)]
    public async Task GetAsync_ValidId_ReturnRight(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _rightManager!.GetAsync(id);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IRight>(x => x.Id == id);

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
        Func<Task> act = async () => await _rightManager!.GetAsync(id);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid right.");

        #endregion
    }

    [Theory]
    [InlineData(new [] { RightConstants.Rights, RightConstants.UserRights })]
    [InlineData(new [] { RightConstants.UserRights })]
    public async Task GetListAsync_ValidIds_ReturnRights(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _rightManager!.GetListAsync(ids);

        #region Assert

        result.Should().NotBeEmpty()
            .And.HaveCount(ids.Length)
            .And.OnlyContain(x => ids.Contains(x.Id));

        #endregion
    }

    [Theory]
    [InlineData(new [] { long.MaxValue, RightConstants.UserRights })]
    [InlineData(new long[] {  })]
    public async Task GetListAsync_InValidIds_ThrowsNotFoundException(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _rightManager!.GetListAsync(ids);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid right.");

        #endregion
    }
}