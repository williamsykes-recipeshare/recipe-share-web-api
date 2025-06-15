namespace RecipeShareLibrary.Model.Rights.Implementation;

public class Right : BaseModel, IRight
{
    public long? ParentId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Url { get; set; }
    public bool? IsMenu { get; set; }
    public required RightsEnum.EnumRightType Type { get; set; }
}