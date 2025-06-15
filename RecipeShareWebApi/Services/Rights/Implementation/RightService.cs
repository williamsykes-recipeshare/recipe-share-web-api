using RecipeShareLibrary.Manager.Rights;
using RecipeShareLibrary.Model.Rights;

namespace RecipeShareWebApi.Services.Rights.Implementation;

public class RightService(IRightManager rightManager) : IRightService
{
    public async Task<IEnumerable<IRight>> GetListAsync(IUser user, CancellationToken cancellationToken)
    {
        if (user.UserRights == null) return new List<IRight>();

        return await rightManager.GetListAsync(user.UserRights.Select(x => x.RightId).ToArray(), cancellationToken);
    }
    public async Task<IEnumerable<IRight>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await rightManager.GetAllAsync(cancellationToken);
    }
}