using RecipeShareWebApi.CustomFilters;
using Microsoft.AspNetCore.Mvc;

namespace RecipeShareWebApi.CustomAttributes;

public class RightsRequirementAttribute : TypeFilterAttribute
{
    public RightsRequirementAttribute(params long[] rightCodes) : base(typeof(RightsRequirementFilter))
    {
        Arguments = new object[] { rightCodes };
    }
}