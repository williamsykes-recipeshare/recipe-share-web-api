namespace RecipeShareLibrary.Model.Rights.Implementation;

public class UserToken : IUserToken
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
    public Guid Guid { get; set; }
    public DateTime ExpirationDate { get; set; }
    public required string Token { get; set; }
    public required string UserAgent { get; set; }
    public required string IpAddress { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}