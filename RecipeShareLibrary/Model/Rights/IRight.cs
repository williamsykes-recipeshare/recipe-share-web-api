namespace RecipeShareLibrary.Model.Rights;

public interface IRight : IBaseModel
{
    long? ParentId { get; set; }
    string Code { get; set; }
    string Name { get; set; }
    string? Url { get; set; }
    bool? IsMenu { get; set; }
    RightsEnum.EnumRightType Type { get; set; }
}