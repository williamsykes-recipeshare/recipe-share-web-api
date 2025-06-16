using FluentAssertions;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Manager.Rights.Implementation;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareTest.Helpers;
using RecipeShareTest.Helpers.Manager;
using Xunit;

namespace RecipeShareTest.Manager.Rights;

public class UserTokenManagerTest : TestWithSettings
{
    private IUserManager? _userManager;
    private IUserTokenManager? _userTokenManager;

    [Fact]
    public async Task DatabaseIsAvailableAndCanBeConnectedTo()
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        Assert.True(await dbContext.Database.CanConnectAsync());
    }

    private async Task SetUp()
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        await TestData.Rights.UserData.SeedData(dbContext);

        _userManager = RightsManagerHelper.CreateUserManager(DbFactory);
        _userTokenManager = new UserTokenManager(DbFactory(), ApplicationSettingsOptions);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GenerateTokenAsync_ValidUser_ReturnToken(long userId)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        var result = await _userTokenManager!.GenerateTokenAsync(user, "", "");

        #region Assert

        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User?.Id.Should().Be(userId);
        result.User?.UserRights.Should().NotBeNull();

        #endregion
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GenerateTokenAsync_ValidUserDifferentAgent_ReturnToken(long userId)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        var result1 = await _userTokenManager!.GenerateTokenAsync(user, "Agent1", "Ip1");
        var result2 = await _userTokenManager!.GenerateTokenAsync(user, "Agent2", "Ip2");

        #region Assert

        result1.Should().NotBe(result2);

        #endregion
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GenerateTokenAsync_ValidUserSameAgent_ReturnToken(long userId)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        var result1 = await _userTokenManager!.GenerateTokenAsync(user, "Agent1", "Ip1");
        var result2 = await _userTokenManager!.GenerateTokenAsync(user, "Agent1", "Ip1");

        #region Assert

        result1.Should().BeEquivalentTo(result2);

        #endregion
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetSessionAsync_ValidUser_ReturnToken(long userId)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);
        var token = await _userTokenManager!.GenerateTokenAsync(user, "", "");

        #endregion

        // Act
        var result = await _userTokenManager!.GetSessionAsync(token.Guid);

        #region Assert

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(token);
        result.User.Should().NotBeNull();
        result.User?.Id.Should().Be(userId);

        #endregion
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task RemoveAsync_ValidUser_ReturnTask(long userId)
    {
        #region Arrange

        await SetUp();
        var user = await _userManager!.GetAsync(userId);
        var token = await _userTokenManager!.GenerateTokenAsync(user, "", "");

        #endregion

        // Act
        await _userTokenManager!.RemoveAsync(token.Guid);

        #region Assert

        Func<Task> act = async () => await _userTokenManager!.GetSessionAsync(token.Guid);

        await act.Should().ThrowAsync<Exception>()
            .Where(x => x.GetType() == typeof(InvalidTokenException))
            .WithMessage("Invalid Token.");

        #endregion
    }
}