using Asp.Versioning;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomAttributes;
using RecipeShareWebApi.Services.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RecipeShareLibrary.Model.Rights.Implementation;
using RecipeShareLibrary.Model.Settings;
using RecipeShareWebApi.Model.Rights;

namespace RecipeShareWebApi.Controllers.Rights;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/User/[action]")]
public class UserController(IUserService userService) : Controller
{
    [HttpGet]
    [Authorize]
    [ActionName("GetList")]
    public async Task<ActionResult<IEnumerable<IUser>>> GetList(CancellationToken cancellationToken = default)
    {
        var result = await userService.GetListAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ApiVersion("1.0")]
    [Authorize]
    [ActionName("Get")]
    [RightsRequirement(RightConstants.UserRights)]
    public async Task<ActionResult<IUser>> Get(long id)
    {
        var result = await userService.GetAsync(id);

        return Ok(result);
    }

    [HttpPost]
    [ApiVersion("1.0")]
    [Authorize]
    [ActionName("Save")]
    [RightsRequirement(RightConstants.UserRights)]
    public async Task<ActionResult<IUser>> Save(User save)
    {
        var result = await userService.SaveAsync(save);

        return Ok(result);
    }

    [HttpPost]
    [ApiVersion("1.0")]
    [Authorize]
    [ActionName("UpdatePassword")]
    public async Task<ActionResult> UpdatePassword([FromForm]string password)
    {
        await userService.UpdatePasswordAsync(password);

        return Ok();
    }

    [HttpPost]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimiterSettings.MyRateLimit)]
    [ActionName("Register")]
    public async Task<ActionResult> Register([FromForm]Register register)
    {
        await userService.RegisterAsync(register.Name, register.Email, register.Password);
        return Ok();
    }

    [HttpDelete]
    [ApiVersion("1.0")]
    [Authorize]
    [ActionName("Delete")]
    public async Task<ActionResult> Register([FromQuery]long id)
    {
        await userService.DeleteAsync(id);
        return Ok();
    }
}