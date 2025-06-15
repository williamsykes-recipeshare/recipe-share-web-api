namespace RecipeShareLibrary.Model.Rights.Implementation;

public class UserVerifyToken : BaseModel, IUserVerifyToken
{
    public long UserId { get; set; }
    public byte[]? Token { get; set; }
    public RightsEnum.EnumUserVerifyType Type { get; set; }
    public string TypeDescription => Type.ToString("G");

    public bool? Expired { get; set; }
    public DateTime ValidTo { get; set; }
}