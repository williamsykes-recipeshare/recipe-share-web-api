namespace RecipeShareLibrary.Model.Rights;

public interface IUserPassword
{
    long Id { get; set; }
    byte[] Password { get; set; }
}