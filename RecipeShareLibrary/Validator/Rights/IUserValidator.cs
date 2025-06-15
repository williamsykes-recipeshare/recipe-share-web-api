using RecipeShareLibrary.Model.Rights;

namespace RecipeShareLibrary.Validator.Rights;

public interface IUserValidator
{
    void ValidatePassword(string password);
    void ValidateNew(IUser user);
    void ValidateSave(IUser user);
    void ValidateRegistration(string name, string email, string password);
}