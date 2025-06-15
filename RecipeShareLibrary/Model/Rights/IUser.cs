using RecipeShareLibrary.Model.Rights.Implementation;

namespace RecipeShareLibrary.Model.Rights;

public interface IUser : IBaseModel
{
    Guid Guid { get; set; }
    string Name { get; set; }
    string Email { get; set; }
    DateTime? LastLogin { get; set; }
    ICollection<UserRight>? UserRights { get; set; }

    UserPassword? UserPassword { get; set; }
}