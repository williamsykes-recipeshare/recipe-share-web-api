using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Settings;

namespace RecipeShareLibrary.Validator.Rights.Implementation;

public class UserValidator(ApplicationSettings settings) : IUserValidator
{
    public void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new BadRequestException("Invalid password.");

        if (password.Length < 8)
            throw new BadRequestException("Invalid password length, should be 8 or more characters.");

        if (!password.Any(char.IsUpper))
            throw new BadRequestException("Invalid password, should contain at least 1 uppercase character.");

        if (!password.Any(char.IsLower))
            throw new BadRequestException("Invalid password, should contain at least 1 lowercase character.");

        if (!password.Any(char.IsNumber))
            throw new BadRequestException("Invalid password, should contain at least 1 number.");
    }

    public void ValidateNew(IUser user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            throw new BadRequestException("Invalid name.");

        if (!RecipeShareUtils.IsValidEmail(user.Email))
            throw new BadRequestException("Invalid email.");

        if (user.UserPassword?.Password == null)
            throw new BadRequestException("Password required.");

        if (user.UserPassword.Password.Length is < 1)
            throw new BadRequestException("Password required.");
    }

    public void ValidateSave(IUser user)
    {
        if (user.Id < 0)
            throw new BadRequestException("Invalid user.");

        if (user.Guid == new Guid())
            throw new BadRequestException("Invalid guid.");

        if (user.Guid == Guid.Empty)
            throw new BadRequestException("Invalid guid.");

        if (string.IsNullOrWhiteSpace(user.Name))
            throw new BadRequestException("Invalid username.");

        if (!RecipeShareUtils.IsValidEmail(user.Email))
            throw new BadRequestException("Invalid email.");

        if (user.UserRights != null && user.UserRights.Select(x => x.RightId).Any(x => x <= 0))
            throw new BadRequestException("Invalid right ID(s).");
    }

    public void ValidateRegistration(string name, string email, string password)
    {
        if (!RecipeShareUtils.IsValidEmail(email))
            throw new BadRequestException("Invalid email.");

        if (string.IsNullOrWhiteSpace(name))
            throw new BadRequestException("Invalid name.");

        if (settings.AuthorisedDomains != null && !settings.AuthorisedDomains!.Contains("*"))
        {
            var domainName = RecipeShareUtils.GetEmailDomain(email);

            if (!settings.AuthorisedDomains!.Contains(domainName))
                throw new BadRequestException("Invalid email domain.");
        }

        ValidatePassword(password);
    }
}