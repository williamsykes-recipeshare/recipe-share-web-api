namespace RecipeShareLibrary.Model;

public interface IBaseModel
{
    long Id { get; set; }
    bool? IsActive { get; set; }
    long CreatedById { get; set; }
    string CreatedByName { get; set; }
    DateTime? CreatedOn { get; set; }
    long UpdatedById { get; set; }
    string UpdatedByName { get; set; }
    DateTime? UpdatedOn { get; set; }
}

public abstract class BaseModel : IBaseModel
{
    public long Id { get; set; }
    public bool? IsActive { get; set; }
    public required long CreatedById { get; set; }
    public required string CreatedByName { get; set; }
    public DateTime? CreatedOn { get; set; }
    public required long UpdatedById { get; set; }
    public required string UpdatedByName { get; set; }
    public DateTime? UpdatedOn { get; set; }
}