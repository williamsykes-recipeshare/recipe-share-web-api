using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.MasterData.Implementation;
using RecipeShareWebApi.Services.MasterData;

namespace RecipeShareWebApi.Controllers.MasterData;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/MasterData/[controller]/[action]")]
public class IngredientController(IIngredientService ingredientService) : Controller
{
    [HttpGet]
    [ActionName("GetList")]
    public async Task<ActionResult<IEnumerable<IIngredient>>> GetList(CancellationToken cancellationToken = default)
    {
        var result = await ingredientService.GetListAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ActionName("Get")]
    public async Task<ActionResult<IIngredient>> Get(long id)
    {
        var result = await ingredientService.GetAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [Authorize]
    [ActionName("Save")]
    public async Task<ActionResult<IIngredient>> Save(Ingredient save)
    {
        var result = await ingredientService.SaveAsync(save);
        return Ok(result);
    }
}