namespace RecipeShareLibrary.Model.MasterData.Implementation;

public class Ingredient : BaseModel, IIngredient
{
    public Guid Guid { get; set; }
    public required string Name { get; set; }
}