using Asp.Versioning;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomAttributes;
using RecipeShareWebApi.Services.Rights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RecipeShareWebApi.Controllers.Rights;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Right/[action]")]
public class RightController(IRightService rightService) : Controller
{
    [HttpGet]
    [Authorize]
    [ActionName("GetList")]
    [RightsRequirement(RightConstants.UserRights)]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Items["User"] is not IUser user) return Unauthorized();

        var resultList = await rightService.GetListAsync(user, cancellationToken);

        return Ok(resultList);
    }

    [HttpGet]
    [Authorize]
    [ActionName("GetAll")]
    [RightsRequirement(RightConstants.UserRights)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Items["User"] is not IUser user) return Unauthorized();

        var resultList = await rightService.GetAllAsync(cancellationToken);

        return Ok(resultList);
    }
}