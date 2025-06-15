using RecipeShareLibrary.Model.Rights.Implementation;

namespace RecipeShareLibrary.Model.Rights;

public interface IUserRight : IBaseModel
{
    long RightId { get; set; }
    Right? Right { get; set; }
    long UserId { get; set; }
}