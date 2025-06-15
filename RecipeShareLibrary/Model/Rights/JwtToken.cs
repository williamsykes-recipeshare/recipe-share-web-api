using System.IdentityModel.Tokens.Jwt;

namespace RecipeShareLibrary.Model.Rights;

public class JwtToken
{
    private readonly JwtSecurityToken _token;

    internal JwtToken(JwtSecurityToken token)
    {
        this._token = token;
    }

    public DateTime ValidTo => _token.ValidTo;
    public string Value => new JwtSecurityTokenHandler().WriteToken(this._token);
}