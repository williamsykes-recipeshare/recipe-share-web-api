using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.DBContext.Implementation;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Rights.Implementation;
using RecipeShareLibrary.Model.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace RecipeShareLibrary.Manager.Rights.Implementation;

public class UserTokenManager(
    IRecipeShareDbContextFactory dbContextFactory,
    IOptions<ApplicationSettings> applicationSettings)
    : IUserTokenManager
{
    private readonly TokenSettings? _tokenSetting = applicationSettings.Value.TokenSettings;

    private static IQueryable<UserToken> GetQuery(RecipeShareDbContext dbContext)
    {
        return dbContext.UserTokens
            .Include(x => x.User)
                .ThenInclude(x => x!.UserRights!.Where(y => y.IsActive == true))
                .ThenInclude(x => x.Right);
    }

    public async Task<IUserToken> GenerateTokenAsync(IUser user, string userAgent, string remoteIp)
    {
        if (_tokenSetting == null) throw new Exception("Settings missing");

        // ReSharper disable once ConvertToUsingDeclaration
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var userToken = await GetQuery(dbContext)
            .FirstOrDefaultAsync(x =>
                x.UserId == user.Id && x.ExpirationDate > DateTime.UtcNow
                                      && x.IsActive == true
                                      && x.UserAgent == userAgent
                                      && x.IpAddress == remoteIp);

        if (userToken == null)
        {
            userToken = new UserToken
            {
                ExpirationDate = DateTime.UtcNow.Add(new TimeSpan(0, _tokenSetting.ExpirationMinutes ?? 0, 0)),
                IsActive = true,
                Token = "",
                UserId = user.Id,
                Guid = Guid.NewGuid(),
                IpAddress = remoteIp,
                UserAgent = userAgent
            };

            dbContext.UserTokens.Attach(userToken);

            var token = JwtTokenBuilder.BuildToken(_tokenSetting, userToken, user);
            userToken.ExpirationDate = token.ValidTo;
            userToken.Token = token.Value;

            await dbContext.SaveChangesAsync();

            await dbContext.Entry(userToken)
                .Reference(x => x.User!)
                .Query()
                .Include(x => x.UserRights!.Where(z => z.IsActive == true))
                .ThenInclude(x => x.Right!)
                .LoadAsync();
        }

        return userToken;
    }

    public async Task RemoveAsync(Guid guid)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var tokens = await dbContext.UserTokens.AsTracking().Where(x => x.Guid == guid).ToListAsync();
        tokens.ForEach(x => x.IsActive = false);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IUserToken> GetSessionAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        IUserToken? session = await GetQuery(dbContext)
            .Where(x => x.Guid == guid)
            .OrderBy(x => x.ExpirationDate)
            .SingleOrDefaultAsync(cancellationToken);

        if (session == null)
            throw new InvalidTokenException("Invalid Token.");

        if (session.IsActive != true)
            throw new InvalidTokenException("Invalid Token.");

        if (session.ExpirationDate < DateTime.UtcNow)
            throw new InvalidTokenException("Invalid Token.");

        return session;
    }
}