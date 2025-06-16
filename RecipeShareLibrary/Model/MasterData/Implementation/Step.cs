using RecipeShareLibrary.Model.Recipes.Implementation;

namespace RecipeShareLibrary.Model.MasterData.Implementation;

public class Step : BaseModel, IStep
{
    public Guid Guid { get; set; }
    public long RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public int Index { get; set; }
    public required string Name { get; set; }
}