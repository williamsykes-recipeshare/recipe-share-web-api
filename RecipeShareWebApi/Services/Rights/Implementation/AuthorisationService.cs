using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomExceptions;

namespace RecipeShareWebApi.Services.Rights.Implementation;

public class AuthorisationService(
    IUserManager userManager,
    IUserTokenManager userTokenManager,
    IHttpContextAccessor httpContextAccessor)
    : IAuthorisationService
{
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;

    public async Task<IUserToken> LogInAsync(string email, string password)
    {
        var userAgent = _httpContext?.Request.Headers.UserAgent.ToString() ?? "";
        var remoteIp = _httpContext?.Connection.RemoteIpAddress?.ToString() ?? "";
        var result = await userManager.AuthenticateUserAsync(email, password, userAgent, remoteIp);

        return result;
    }

    public async Task LogoutAsync()
    {
        var tokenIdClaim = _httpContext?.User.FindFirst("jti");

        if (tokenIdClaim == null || !Guid.TryParse(tokenIdClaim.Value, out var guid)) throw new UnauthorisedException("Unauthorised");

        await userTokenManager.RemoveAsync(guid);
    }

    public async Task<IUserToken> GetSessionAsync(CancellationToken cancellationToken)
    {
        var tokenIdClaim = _httpContext?.User.FindFirst("jti");

        if (tokenIdClaim == null || !Guid.TryParse(tokenIdClaim.Value, out var guid)) throw new UnauthorisedException("Unauthorised");

        var result = await userTokenManager.GetSessionAsync(guid, cancellationToken);

        return result;
    }
}