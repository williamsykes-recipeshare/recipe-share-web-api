using System;
using RecipeShareLibrary.Model.Rights.Implementation;

namespace RecipeShareLibrary.Model.Rights;

public interface IUserToken
{
    long Id { get; set; }
    long UserId { get; set; }
    User? User { get; set; }
    Guid Guid { get; set; }
    DateTime ExpirationDate { get; set; }
    string Token { get; set; }
    string UserAgent { get; set; }
    string IpAddress { get; set; }
    bool? IsActive { get; set; }
    DateTime? CreatedOn { get; set; }
    DateTime? UpdatedOn { get; set; }
}