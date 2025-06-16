using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareWebApi.CustomAttributes;
using RecipeShareWebApi.Services.MasterData;

namespace RecipeShareWebApi.Controllers.MasterData;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/MasterData/[controller]/[action]")]
public class DietaryTagController(IDietaryTagService dietaryTagService) : Controller
{
    [HttpGet]
    [Authorize]
    [ActionName("GetList")]
    public async Task<ActionResult<IEnumerable<IDietaryTag>>> GetList(CancellationToken cancellationToken = default)
    {
        var result = await dietaryTagService.GetListAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    [ActionName("Get")]
    public async Task<ActionResult<IDietaryTag>> Get(long id)
    {
        var result = await dietaryTagService.GetAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [Authorize]
    [ActionName("Save")]
    // [RightsRequirement(RightConstants.MasterData)]
    public async Task<ActionResult<IDietaryTag>> Save(DietaryTag save)
    {
        var result = await dietaryTagService.SaveAsync(save);
        return Ok(result);
    }
}