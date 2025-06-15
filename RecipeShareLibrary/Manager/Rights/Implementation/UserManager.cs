using System.Text;
using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Validator.Rights;

namespace RecipeShareLibrary.Manager.Rights.Implementation;

public class UserManager(
    IRecipeShareDbContextFactory dbContextFactory,
    IUserTokenManager userTokenManager,
    IUserValidator userValidator,
    IRightManager rightManager)
    : IUserManager
{
    private static IQueryable<User> GetQuery(RecipeShareDbContext dbContext)
    {
        return dbContext.Users
            .Include(x => x.UserRights!)
                .ThenInclude(x => x.Right);
    }

    public async Task RegisterUserAsync(string name, string email, string password)
    {
        userValidator.ValidateRegistration(name, email, password);

        var existingUser = await GetAsync(email);
        if (existingUser != null)
        {
            throw new BadRequestException("Email address already in use.");
        }

        IUser user = new User
        {
            Id = 0,
            Name = name,
            Email = email.ToLowerInvariant(),
            UserPassword = new UserPassword()
            {
                Id = 0,
                Password = Encoding.UTF8.GetBytes(password),
            },
            Guid = Guid.NewGuid(),
            CreatedById = 0,
            CreatedByName = "",
            UpdatedById = 0,
            UpdatedByName = "",
        };

        user = await SaveAsync(new User
        {
            Id = 0,
            Name = "Self Registration",
            Email = "",
            CreatedById = 0,
            CreatedByName = "",
            UpdatedById = 0,
            UpdatedByName = "",
        }, user);
    }

    public async Task<IUserToken> AuthenticateUserAsync(string email, string password, string userAgent, string remoteIp)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            // User does not exist.
            throw new BadRequestException("Invalid email / password.");
        }

        var user = await GetAsync(email);

        if (user == null)
        {
            // User does not exist.
            throw new BadRequestException("Invalid email / password.");
        }

        if (user.UserPassword == null)
        {
            // User password was not included
            throw new BadRequestException("Invalid email / password.");
        }

        // Check explicitly if not true as it can be null
        if (user.IsActive != true)
        {
            // User has been de-activated.
            throw new BadRequestException("Invalid email / password.");
        }

        // Incorrect password.
        if (!PasswordHelper.VerifyHashedPassword(user.UserPassword, password))
            throw new BadRequestException("Invalid email / password.");

        // Generally we would ignore this task, but it breaks tests,
        // as it closes the database.
        await UpdateLastLoginAsync(user.Id);

        return await userTokenManager.GenerateTokenAsync(user, userAgent, remoteIp);

    }

    public async Task<IUser?> GetAsync(string email)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var result = await GetQuery(dbContext)
            .Include(x => x.UserPassword)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant());

        return result;
    }

    public async Task<IUser> GetAsync(long userId, CancellationToken cancellationToken = default)
    {
        var users = await GetListAsync([userId], cancellationToken);

        return users.First();
    }

    public async Task<IUser> SaveAsync(IUser user, IUser save)
    {
        userValidator.ValidateSave(save);

        if (save.UserRights?.Any() == true) await rightManager.GetListAsync(save.UserRights.Select(x => x.RightId).ToArray());

        IUser? result;
        if (save.Id == 0)
        {
            var existingUser = await GetAsync(save.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("A user with the specified email already exists.");
            }

            userValidator.ValidateNew(save);

            result = new User
            {
                Guid = save.Guid,
                Email = save.Email,
                UserRights = new List<UserRight>(),
                UserPassword =  new UserPassword()
                {
                    Id = 0,
                    Password = [],
                },
                Name = "",
                CreatedById = user.Id,
                CreatedByName = user.Name,
                UpdatedById = user.Id,
                UpdatedByName = user.Name,
            };
        }
        else
        {
            result = await GetAsync(save.Id);

            if (result == null) throw new Exception("Invalid");

            // If the email is being updated, make sure it isn't being updated to an email that is already used for another user.
            if (save.Email != result.Email)
            {
                var existingUser = await GetAsync(save.Email);
                if (existingUser != null)
                {
                    throw new BadRequestException("A user with the specified email already exists.");
                }
            }
        }

        #region Update User

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Attach(result);

        #region User Password

        if (save.UserPassword?.Password.Length is > 0)
        {
            var password = Encoding.ASCII.GetString(save.UserPassword.Password);
            userValidator.ValidatePassword(password);

            PasswordHelper.HashPassword(password, out var passwordHash);

            if (save.Id > 0)
            {
                await dbContext.Entry(result).Reference(x => x.UserPassword!)
                    .LoadAsync();
            }

            result.UserPassword!.Password = passwordHash;
        }

        #endregion

        result.Name = save.Name;
        result.Email = save.Email;

        #region Rights

        if (result.UserRights != null && save.UserRights != null)
        {
            foreach (var resultUserRight in result.UserRights)
            {
                resultUserRight.IsActive = false;
                resultUserRight.UpdatedById = user.Id;
                resultUserRight.UpdatedByName = user.Name;
            }

            foreach (var saveUserRight in save.UserRights)
            {
                var userRight = result.UserRights.SingleOrDefault(x => x.RightId == saveUserRight.RightId);

                if (userRight == null)
                {
                    userRight = new UserRight
                    {
                        RightId = saveUserRight.RightId,
                        CreatedById = user.Id,
                        CreatedByName = user.Name,
                        UpdatedById = user.Id,
                        UpdatedByName = user.Name,
                    };

                    result.UserRights.Add(userRight);
                }

                userRight.IsActive = true;
                userRight.UpdatedById = user.Id;
                userRight.UpdatedByName = user.Name;
            }
        }

        #endregion

        result.UpdatedById = user.Id;
        result.UpdatedByName = user.Name;

        #endregion

        await dbContext.SaveChangesAsync();

        await dbContext.Entry(result).Collection(x => x.UserRights!)
            .Query()
            .Include(x => x.Right!)
            .LoadAsync();

        // I do this only here, because password shouldn't return when saved.
        result.UserPassword = null;

        return result;
    }

    public async Task<IEnumerable<IUser>> GetListAsync(long[] ids, CancellationToken cancellationToken = default)
    {
        if (ids.Length == 0)
            throw new NotFoundException("Invalid user.");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        // We ignore the warning since sql injection isn't a concern here.
#pragma warning disable EF1002
        var resultQuery = dbContext.Users
            .FromSqlRaw($@"
                SELECT * FROM rgh_user
                WHERE usrId IN ({string.Join(',', ids)})
             ")
#pragma warning restore EF1002
            .Include(x => x.UserRights!)
            .ThenInclude(x => x.Right);

        if (ids.Distinct().Count() != await resultQuery.CountAsync(cancellationToken))
            throw new NotFoundException("Invalid user.");

        var resultList = await resultQuery.ToListAsync(cancellationToken);

        return resultList.WithoutPasswords();
    }

    public async Task<IEnumerable<IUser>> GetListAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var resultList = await dbContext.Users
            .Include(x => x.UserRights!)
            .ThenInclude(x => x.Right)
            .ToListAsync(cancellationToken);

        return resultList.WithoutPasswords();
    }

    public async Task<IUser> UpdateLastLoginAsync(long userId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        IUser? user = await dbContext.Users
            .SingleOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new BadRequestException("Invalid user.");

        dbContext.Attach(user);

        user.LastLogin = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return user.WithoutPassword();
    }

    public async Task UpdatePasswordAsync(IUser user, string password)
    {
        userValidator.ValidatePassword(password);

        PasswordHelper.HashPassword(password, out var passwordHash);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var result = await GetQuery(dbContext)
            .Include(x => x.UserPassword)
            .AsTracking()
            .OrderBy(x => x.Id)
            .SingleAsync(x => x.Id == user.Id);

        result.UserPassword!.Password = passwordHash;

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdatePasswordAsync(IUser updatedBy, long userId, string password)
    {
        userValidator.ValidatePassword(password);

        PasswordHelper.HashPassword(password, out var passwordHash);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var result = await GetQuery(dbContext)
            .Include(x => x.UserPassword)
            .AsTracking()
            .OrderBy(x => x.Id)
            .SingleAsync(x => x.Id == userId);

        result.UserPassword!.Password = passwordHash;

        result.UpdatedById = updatedBy.Id;
        result.UpdatedByName = updatedBy.Name;
        result.UpdatedOn = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(IUser user, long id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var userToDelete = await dbContext.Users
            .Include(x => x.UserRights)
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();

        if (userToDelete == null)
            throw new NotFoundException("User not found.");

        var userTokens = await dbContext.UserTokens
            .Where(x => x.IsActive.HasValue && x.IsActive.Value == true && x.UserId == id)
            .ToListAsync();

        dbContext.Attach(userToDelete);

        userToDelete.IsActive = false;
        userToDelete.UpdatedById = user.Id;
        userToDelete.UpdatedByName = user.Name;

        foreach (var userRight in userToDelete.UserRights)
        {
            userRight.IsActive = false;
            userRight.UpdatedById = user.Id;
            userRight.UpdatedByName = user.Name;
        }

        foreach (var userToken in userTokens)
        {
            userToken.IsActive = false;
        }

        await dbContext.SaveChangesAsync();
    }
}