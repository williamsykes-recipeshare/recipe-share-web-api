using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RecipeShareWebApi.Helper;

public class JwtSecurityKey
{
    public static SymmetricSecurityKey Create(string secret)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    }
}