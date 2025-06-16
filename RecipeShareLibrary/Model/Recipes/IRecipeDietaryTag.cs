using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareLibrary.Model.Recipes;

public interface IRecipeDietaryTag : IBaseModel
{
    long DietaryTagId { get; set; }
    DietaryTag? DietaryTag { get; set; }
    long RecipeId { get; set; }
}