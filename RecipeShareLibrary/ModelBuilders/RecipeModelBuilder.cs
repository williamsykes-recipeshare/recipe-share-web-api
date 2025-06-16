using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.ModelBuilders.Recipes;

namespace RecipeShareLibrary.ModelBuilders;

public static class RecipesModelBuilder
{
    public static void BuildRecipes(this ModelBuilder modelBuilder)
    {
        RecipeModelBuilder.Build(modelBuilder);
        RecipeIngredientModelBuilder.Build(modelBuilder);
        RecipeDietaryTagModelBuilder.Build(modelBuilder);
    }
}