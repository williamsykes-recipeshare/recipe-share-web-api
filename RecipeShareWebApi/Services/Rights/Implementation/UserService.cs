using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomExceptions;

namespace RecipeShareWebApi.Services.Rights.Implementation;

public class UserService(IUserManager userManager, IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;

    public async Task<IEnumerable<IUser>> GetListAsync(CancellationToken cancellationToken)
    {
        return await userManager.GetListAsync(cancellationToken);
    }

    public async Task<IUser> GetAsync(long id)
    {
        var result = await userManager.GetAsync(id);
        return result;
    }

    public async Task<IUser> SaveAsync(IUser save)
    {
        if (_httpContext?.Items["User"] is not IUser user) throw new ForbiddenException("");

        return await userManager.SaveAsync(user, save);
    }

    public async Task UpdatePasswordAsync(string password)
    {
        if (_httpContext?.Items["User"] is not IUser user) throw new ForbiddenException("");

        await userManager.UpdatePasswordAsync(user, password);
    }

    public async Task RegisterAsync(string name, string email, string password)
    {
        await userManager.RegisterUserAsync(name, email, password);
    }

    public async Task DeleteAsync(long id)
    {
        if (_httpContext?.Items["User"] is not IUser user) throw new ForbiddenException("");

        await userManager.DeleteAsync(user, id);
    }
}