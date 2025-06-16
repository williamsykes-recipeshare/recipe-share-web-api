using FluentAssertions;
using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Settings;
using RecipeShareTest.Helpers;
using RecipeShareTest.Helpers.Manager;
using RecipeShareTest.TestData.Arguments.Rights;
using RecipeShareTest.TestData.Rights;
using Xunit;

namespace RecipeShareTest.Manager.Rights;

public class UserManagerTest : TestWithSettings
{
    private IUserManager? _userManager;

    [Fact]
    public async Task DatabaseIsAvailableAndCanBeConnectedTo()
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        Assert.True(await dbContext.Database.CanConnectAsync());
    }

    private async Task SetUp(ApplicationSettings? applicationSettings = null)
    {
        await using var dbContext = await DbFactory().CreateDbContextAsync();
        await RightData.SeedData(dbContext);
        await UserData.SeedData(dbContext);
        await UserRightData.SeedData(dbContext);

        _userManager = RightsManagerHelper.CreateUserManager(DbFactory, applicationSettings);
    }

    [Fact]
    public async Task GetListAsync_NoArguments_ReturnsAllUsers()
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.GetListAsync();

        #region Assert

        result.Should().NotBeEmpty();

        #endregion
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetAsync_ValidId_ReturnsUser(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.GetAsync(id);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IUser>(x => x.UserRights != null);

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
        Func<Task> act = async () => await _userManager!.GetAsync(id);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid user.");

        #endregion
    }



    [Theory]
    [InlineData(new long[] { 0, 1, 2, 3 })]
    [InlineData(new [] { long.MaxValue, 2 })]
    [InlineData(new long[] { })]
    public async Task GetListAsync_InvalidIds_ThrowsNotFoundException(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.GetListAsync(ids);

        #region Assert

        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Invalid user.");

        #endregion
    }

    [Theory]
    [InlineData(new long[] { 1, 2, 3, 4, 5 })]
    [InlineData(new long[] { 2 })]
    public async Task GetListAsync_ValidIds_ReturnUsers(long[] ids)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.GetListAsync(ids);

        #region Assert

        result.Should().NotBeEmpty()
            .And.HaveCount(ids.Length)
            .And.OnlyContain(x => ids.Contains(x.Id))
            .And.OnlyContain(x => x.UserPassword == null);

        #endregion
    }

    [Theory]
    [InlineData("user1@mail.com")]
    [InlineData("user2@mail.com")]
    [InlineData("user3@mail.com")]
    [InlineData("user4@mail.com")]
    public async Task GetAsync_ValidEmail_ReturnsUser(string email)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.GetAsync(email);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IUser>(x => x.UserRights != null)
            .And.Match<IUser>(x => x.UserPassword != null);

        #endregion
    }

    [Theory]
    [InlineData("user7@mail.com")]
    [InlineData("")]
    [InlineData("lalala")]
    public async Task GetAsync_InValidEmail_ReturnsNull(string email)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.GetAsync(email);

        #region Assert

        result.Should().BeNull();

        #endregion
    }

    [Theory]
    [ClassData(typeof(UserValidateSaveArgs))]
    public async Task SaveAsync_InValidUser_ThrowsBadRequest(long userId, IUser save, string exception, Type exceptionType)
    {
        #region Arrange

        await SetUp();

        var user = await _userManager!.GetAsync(userId);

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.SaveAsync(user, save);

        // Assert
        await act.Should().ThrowAsync<Exception>(exception)
            .Where(x => x.GetType() == exceptionType, exception)
            .WithMessage(exception);
    }

    [Fact]
    public async Task SaveAsync_ValidNew_InsertsRecord()
    {
        #region Arrange

        await SetUp();

        var user = await _userManager!.GetAsync(1);
        var save = UserData.CreateNewSave();
        #endregion

        // Act
        var result = await _userManager!.SaveAsync(user, save);

        #region Assert
        result.Id.Should().BeGreaterThan(0);
        result.UserPassword.Should().BeNull();
        result.BeValidAudit(save, user);

        result.Id.Should().Be(result.Id);

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
                    .Excluding(x => x.IsActive)
                    .Excluding(x => x.UserPassword)
                    .Excluding(x => x.Id)
                    .Excluding(x => x.UserRights));

        result.UserRights.Should()
            .OnlyContain(x => x.UserId == result.Id)
            .And.OnlyContain(x => x.Right != null);

        var inserted = await _userManager!.GetAsync(result.Id);

        result.Should().NotBeNull()
            .And.BeEquivalentTo(inserted, config =>
                config.Excluding(x => x.UserPassword));

        if (save.UserRights != null)
        {
            inserted.UserRights.Should().NotBeNull();

            inserted.UserRights.Should().NotBeEmpty()
                .And.HaveSameCount(save.UserRights)
                .And.OnlyContain(x => x.UserId == result.Id)
                .And.BeEquivalentTo(save.UserRights, config =>
                    config
                        .Excluding(x => x.Right)
                        .Excluding(x => x.UserId)
                        .Excluding(x => x.Id)
                        .Excluding(x => x.CreatedById)
                        .Excluding(x => x.CreatedByName)
                        .Excluding(x => x.CreatedOn)
                        .Excluding(x => x.UpdatedById)
                        .Excluding(x => x.UpdatedByName)
                        .Excluding(x => x.UpdatedOn)
                        .Excluding(x => x.IsActive));
        }

        #endregion
    }

    [Fact]
    public async Task SaveAsync_ValidExisting_SavedRecord()
    {
        #region Arrange

        await SetUp();

        var user = await _userManager!.GetAsync(1);
        var save = UserData.CreateExistingSave();

        #endregion

        // Act
        var result = await _userManager!.SaveAsync(user, save);

        #region Assert
        result.BeValidAudit(save, user);
        result.Id.Should().BeGreaterThan(0);

        var insertedUser = await _userManager!.GetAsync(result.Id);

        insertedUser.Should().NotBeNull()
            .And.BeEquivalentTo(save, config => config
                .Excluding(x => x.Id)
                .Excluding(x => x.UserRights)
                .Excluding(x => x.UserPassword)
                .Excluding(x => x.CreatedOn)
                .Excluding(x => x.UpdatedOn)
                .Excluding(x => x.IsActive));

        insertedUser.UserRights.Should().NotBeNull();
        insertedUser.UserRights!.Where(x => x.IsActive == true).Should().HaveCount(save.UserRights!.Count);

        insertedUser.UserRights!.Where(x => x.IsActive == true).Should()
            .OnlyContain(x => save.UserRights.Any(y => x.RightId == y.RightId));
        #endregion
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(long.MaxValue)]
    public async Task UpdateLastLoginAsync_InValidId_ThrowsBadRequestException(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.UpdateLastLoginAsync(id);

        #region Assert

        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid user.");

        #endregion
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task UpdateLastLoginAsync_ValidIdValidUrl_ReturnsUser(long id)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.UpdateLastLoginAsync(id);

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IUser>(x => x.UserRights == null)
            .And.Match<IUser>(x => x.Id == id);

        #endregion
    }

    [Theory]
    [InlineData("user1@mail.com", "Password124")]
    [InlineData("user1@mail.com", "")]
    [InlineData("", "Password123")]
    [InlineData("", "")]
    [InlineData("user3@mail.com", "Password123")]
    public async Task AuthenticateUserAsync_InValidEmailPassword_ThrowsBadRequestException(string email, string password)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.AuthenticateUserAsync(email, password, "", "");

        #region Assert

        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid email / password.");

        #endregion
    }

    [Theory]
    [InlineData("user1@mail.com", "Password123")]
    [InlineData("user2@mail.com", "Password123")]
    [InlineData("user4@mail.com", "Password123")]
    public async Task AuthenticateUserAsync_ValidEmailValidPassword_ReturnsUserToken(string email, string password)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result = await _userManager!.AuthenticateUserAsync(email, password, "", "");

        #region Assert

        result.Should().NotBeNull()
            .And.Match<IUserToken>(x => x.User != null)
            .And.Match<IUserToken>(x => x.User!.UserPassword == null)
            .And.Match<IUserToken>(x => x.User!.UserRights != null);

        #endregion
    }

    [Fact]
    public async Task AuthenticateUserAsync_DifferentIpAndAgent_ReturnsSameUserToken()
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result1 = await _userManager!.AuthenticateUserAsync("user1@mail.com", "Password123", "Agent1", "IP1");
        var result2 = await _userManager!.AuthenticateUserAsync("user1@mail.com", "Password123", "Agent2", "IP2");

        #region Assert

        result1.Should().NotBe(result2);

        #endregion
    }

    [Fact]
    public async Task AuthenticateUserAsync_SameIpAndAgent_ReturnsSameUserToken()
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        var result1 = await _userManager!.AuthenticateUserAsync("user1@mail.com", "Password123", "Agent1", "IP1");
        var result2 = await _userManager!.AuthenticateUserAsync("user1@mail.com", "Password123", "Agent1", "IP1");

        #region Assert

        result1.Should().BeEquivalentTo(result2, config => config
            .Excluding(x => x.User!.LastLogin));

        #endregion
    }

    [Theory]
    [InlineData("")]
    [InlineData("password124")]
    [InlineData("Password")]
    public async Task UpdatePasswordAsync_InValidPassword_ThrowsBadRequestException(string password)
    {
        #region Arrange

        await SetUp();
        var user = UserData.CreateExistingSave();
        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.UpdatePasswordAsync(user, password);

        #region Assert

        await act.Should().ThrowAsync<BadRequestException>();

        #endregion
    }

    [Fact]
    public async Task UpdatePasswordAsync_ValidPassword_Void()
    {
        #region Arrange

        await SetUp();

        var user = UserData.CreateExistingSave();
        var password = "1234aB!?";

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.UpdatePasswordAsync(user, password);

        #region Assert

        await act.Should().NotThrowAsync();

        Func<Task<IUserToken>> tokenFunc = async () => await _userManager!.AuthenticateUserAsync(user.Email, password, "", "");

        await tokenFunc.Should().NotThrowAsync();
        var token = await tokenFunc();

        #endregion
    }

    [Theory]
    [ClassData(typeof(UserPasswordChangeArgs))]
    public async Task UpdatePasswordAsync_InValidPassword_ThrowsBadRequest(long updateUserId, long userId, string password, string exception, Type exceptionType)
    {
        #region Arrange

        await SetUp();

        var user = await _userManager!.GetAsync(updateUserId);

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.UpdatePasswordAsync(user, userId, password);

        // Assert
        await act.Should().ThrowAsync<Exception>(exception)
            .Where(x => x.GetType() == exceptionType, exception)
            .WithMessage(exception);
    }

    [Fact]
    public async Task UpdatePasswordAsync_ValidUserPassword_Void()
    {
        #region Arrange

        await SetUp();

        var user = UserData.CreateExistingSave();
        var password = "1234aB!?";

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.UpdatePasswordAsync(user, 2, password);

        #region Assert

        await act.Should().NotThrowAsync();

        Func<Task<IUserToken>> tokenFunc = async () => await _userManager!.AuthenticateUserAsync(user.Email, password, "", "");

        await tokenFunc.Should().NotThrowAsync();
        var token = await tokenFunc();

        #endregion
    }

    [Theory]
    [InlineData("Some Name", "ABcd1234@mail.com", "P@ssw0rd123")]
    public async Task RegisterUserAsync_InValidUserEmail_ReturnToken(string name, string email, string password)
    {
        #region Arrange

        var appSettings = ApplicationSettingsOptions.Value;
        appSettings.AuthorisedDomains = ["gmail.com"];
        await SetUp(appSettings);

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.RegisterUserAsync(name, email, password);

        #region Assert

        await act.Should().ThrowAsync<BadRequestException>("Invalid email domain.")
            .WithMessage("Invalid email domain.");;

        #endregion
    }

    [Theory]
    [InlineData("Some Name", "ABcd1234@mail.com", "P@ssw0rd123")]
    [InlineData("Some Name", "ABcd1234@mail.co.za", "P@ssw0rd123")]
    public async Task RegisterUserAsync_ValidUserEmail_ReturnTask(string name, string email, string password)
    {
        #region Arrange

        var appSettings = ApplicationSettingsOptions.Value;
        appSettings.AuthorisedDomains = ["mail.com", "mail.co.za"];
        await SetUp(appSettings);

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.RegisterUserAsync(name, email, password);

        #region Assert

        await act.Should().NotThrowAsync();

        var user = await _userManager!.GetAsync(email);
        user.Should().NotBeNull();

        #endregion
    }

    [Theory]
    [ClassData(typeof(UserValidRegisterArgs))]
    public async Task RegisterUserAsync_InValidUser_ThrowsBadRequest(string name, string email, string password, string exception, Type exceptionType)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.RegisterUserAsync(name, email, password);

        // Assert
        await act.Should().ThrowAsync<Exception>(exception)
            .Where(x => x.GetType() == exceptionType, exception)
            .WithMessage(exception);
    }

    [Theory]
    [InlineData("Some Name", "ABcd1234@mail.com", "P@ssw0rd123")]
    public async Task RegisterUserAsync_ValidUser_ReturnTask(string name, string email, string password)
    {
        #region Arrange

        await SetUp();

        #endregion

        // Act
        Func<Task> act = async () => await _userManager!.RegisterUserAsync(name, email, password);

        #region Assert

        await act.Should().NotThrowAsync();

        var user = await _userManager!.GetAsync(email);
        user.Should().NotBeNull();

        #endregion
    }
}