using RecipeShareLibrary.Model.Rights;
using RecipeShareWebApi.CustomExceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RecipeShareWebApi.CustomFilters;

public class RightsRequirementFilter(ICollection<long> rightIds) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["User"] is not IUser user)
        {
            throw new UnauthorisedException("Unauthorised");
        }

        var rightClaims = (from firstItem in user.UserRights?.Where(x => x.IsActive == true).ToList()
            join secondItem in rightIds
                on firstItem.RightId equals secondItem
            select firstItem);

        if (!rightClaims.Any())
        {
            throw new ForbiddenException("Unauthorised");
        }
    }
}