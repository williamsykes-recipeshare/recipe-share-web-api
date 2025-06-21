namespace RecipeShareLibrary.Model.Rights;

public abstract class RightsEnum
{
    public enum EnumRightType
    {
        Right = 1,
        MasterData = 2,
        Project = 3,
    }

    public enum EnumUserRole
    {
        Admin = 1,
        User = 2,
    }
}