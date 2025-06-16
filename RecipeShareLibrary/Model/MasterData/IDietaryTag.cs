namespace RecipeShareLibrary.Model.MasterData;

public interface IDietaryTag : IBaseModel
{
    Guid Guid { get; set; }
    string Name { get; set; }
}