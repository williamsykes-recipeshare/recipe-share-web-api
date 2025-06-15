using Asp.Versioning;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Settings;
using RecipeShareWebApi.Services.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RecipeShareWebApi.Model.Rights;

namespace RecipeShareWebApi.Controllers.Rights;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Authorisation/[action]")]
public class AuthorisationController(IAuthorisationService authorisationService) : Controller
{
    [HttpPost]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimiterSettings.MyRateLimit)]
    [ActionName("LogIn")]
    public async Task<ActionResult<IUserToken>> LogInAsync(Login loginAuthViewModel)
    {
        var result = await authorisationService.LogInAsync(loginAuthViewModel.Email, loginAuthViewModel.Password);
        return Ok(result);
    }

    [HttpGet]
    [ApiVersion("1.0")]
    [Authorize]
    [ActionName("Logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await authorisationService.LogoutAsync();

        return NoContent();
    }

    [HttpGet]
    [ApiVersion("1.0")]
    [Authorize]
    [ActionName("GetSession")]
    public async Task<IActionResult> GetSessionAsync(CancellationToken cancellationToken = default)
    {
        var result = await authorisationService.GetSessionAsync(cancellationToken);
        return Ok(result);

    }
}