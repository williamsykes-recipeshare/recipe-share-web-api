using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Model.Recipes.Implementation;
using RecipeShareWebApi.Services.Recipes;

namespace RecipeShareWebApi.Controllers.Recipes;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class RecipeController(IRecipeService recipeService) : Controller
{
    [HttpGet]
    [ActionName("GetList")]
    public async Task<ActionResult<IEnumerable<IRecipe>>> GetList(CancellationToken cancellationToken = default)
    {
        var result = await recipeService.GetListAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ActionName("GetFilteredList")]
    public async Task<ActionResult<IEnumerable<IRecipe>>> GetFilteredList([FromBody] RecipeFilters filters, CancellationToken cancellationToken = default)
    {
        var result = await recipeService.GetFilteredListAsync(filters, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ActionName("Get")]
    public async Task<ActionResult<IRecipe>> Get(long id)
    {
        var result = await recipeService.GetAsync(id);
        return Ok(result);
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [Authorize]
    [ActionName("Save")]
    public async Task<ActionResult<IRecipe>> Save(Recipe save)
    {
        var result = await recipeService.SaveAsync(save);
        return Ok(result);
    }
}