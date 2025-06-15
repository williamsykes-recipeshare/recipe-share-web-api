namespace RecipeShareLibrary.Model.Rights.Implementation;

public class UserPassword : IUserPassword
{
    public long Id { get; set; }
    public required byte[] Password { get; set; }
}