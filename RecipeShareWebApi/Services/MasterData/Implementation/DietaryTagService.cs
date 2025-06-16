using RecipeShareLibrary.Manager.MasterData;
using RecipeShareLibrary.Model.MasterData;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomExceptions;

namespace RecipeShareWebApi.Services.MasterData.Implementation;

public class DietaryTagService(IDietaryTagManager dietaryTagManager, IHttpContextAccessor httpContextAccessor) : IDietaryTagService
{
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;

    public async Task<IDietaryTag> GetAsync(long id)
    {
        return await dietaryTagManager.GetAsync(id);
    }

    public async Task<IEnumerable<IDietaryTag>> GetListAsync(CancellationToken cancellationToken)
    {
        return await dietaryTagManager.GetListAsync(cancellationToken);
    }

    public async Task<IDietaryTag> SaveAsync(IDietaryTag save)
    {
        if (_httpContext?.Items["User"] is not IUser user) throw new ForbiddenException("");

        return await dietaryTagManager.SaveAsync(user, save);
    }
}