using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.ModelBuilders.MasterData;

namespace RecipeShareLibrary.ModelBuilders;

public static class MasterDataModelBuilder
{
    public static void BuildMasterData(this ModelBuilder modelBuilder)
    {
        IngredientModelBuilder.Build(modelBuilder);
        StepModelBuilder.Build(modelBuilder);
    }
}