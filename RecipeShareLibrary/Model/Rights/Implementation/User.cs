namespace RecipeShareLibrary.Model.Rights.Implementation;

public class User : BaseModel, IUser
{
    public Guid Guid { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public DateTime? LastLogin { get; set; }

    public ICollection<UserRight>? UserRights { get; set; }

    public UserPassword? UserPassword { get; set; }
}