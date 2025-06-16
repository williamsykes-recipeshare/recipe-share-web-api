namespace RecipeShareLibrary.Model.MasterData.Implementation;

public class DietaryTag : BaseModel, IDietaryTag
{
    public Guid Guid { get; set; }
    public required string Name { get; set; }
}