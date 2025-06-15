namespace RecipeShareLibrary.Model.Rights;

public interface IUserVerifyToken : IBaseModel
{
    long UserId { get; set; }
    byte[]? Token { get; set; }
    RightsEnum.EnumUserVerifyType Type { get; set; }
    string TypeDescription { get; }
    bool? Expired { get; set; }
    DateTime ValidTo { get; set; }
}