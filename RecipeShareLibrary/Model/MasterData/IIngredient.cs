namespace RecipeShareLibrary.Model.MasterData;

public interface IIngredient : IBaseModel
{
    Guid Guid { get; set; }
    string Name { get; set; }
}