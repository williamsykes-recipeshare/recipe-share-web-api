using RecipeShareLibrary.ModelBuilders.Rights;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.ModelBuilders;

public static class RightsModelBuilder
{
    public static void BuildRights(this ModelBuilder modelBuilder)
    {
        RightModelBuilder.Build(modelBuilder);
        UserModelBuilder.Build(modelBuilder);
        UserRightModelBuilder.Build(modelBuilder);
        UserTokenModelBuilder.Build(modelBuilder);
    }
}