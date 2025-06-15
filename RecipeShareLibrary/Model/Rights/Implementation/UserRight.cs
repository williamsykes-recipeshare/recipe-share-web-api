namespace RecipeShareLibrary.Model.Rights.Implementation;

public class UserRight : BaseModel, IUserRight
{
    public long RightId { get; set; }
    public Right? Right { get; set; }
    public long UserId { get; set; }
}