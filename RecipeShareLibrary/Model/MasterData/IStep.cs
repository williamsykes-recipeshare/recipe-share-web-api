namespace RecipeShareLibrary.Model.MasterData;

public interface IStep : IBaseModel
{
    Guid Guid { get; set; }
    long RecipeId { get; set; }
    int Index { get; set; }
    string Name { get; set; }
}