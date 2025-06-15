using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RecipeShareLibrary.Model.Rights;
using RecipeShareLibrary.Model.Settings;
using Microsoft.IdentityModel.Tokens;

namespace RecipeShareLibrary.Helper;

public class JwtTokenBuilder
{
    private SecurityKey? _securityKey;
    private string _subject = "";
    private string _issuer = "";
    private string _audience = "";
    private readonly List<KeyValuePair<string, string>> _claims = new ();
    private int _expiryInMinutes = 5;

    private JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
    {
        _securityKey = securityKey;
        return this;
    }

    private JwtTokenBuilder AddSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    private JwtTokenBuilder AddIssuer(string issuer)
    {
        _issuer = issuer;
        return this;
    }

    private JwtTokenBuilder AddAudience(string audience)
    {
        _audience = audience;
        return this;
    }

    private JwtTokenBuilder AddClaim(string type, string value)
    {
        _claims.Add(new KeyValuePair<string, string>(type, value));
        return this;
    }

    public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
    {
        _claims.AddRange(claims);
        return this;
    }

    private JwtTokenBuilder AddExpiry(int expiryInMinutes)
    {
        this._expiryInMinutes = expiryInMinutes;
        return this;
    }

    private JwtToken Build()
    {
        EnsureArguments();

        var claims = new List<Claim>
        {
          new Claim(JwtRegisteredClaimNames.Sub, this._subject),
        }
        .Union(this._claims.Select(item => new Claim(item.Key, item.Value)));

        var token = new JwtSecurityToken(
                          issuer: this._issuer,
                          audience: this._audience,
                          claims: claims,
                          expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                          signingCredentials: new SigningCredentials(
                                                    this._securityKey,
                                                    SecurityAlgorithms.HmacSha256));

        return new JwtToken(token);
    }

    public static JwtToken BuildToken(TokenSettings tokenSetting, IUserToken? userToken, IUser? user)
    {
        var tokenBuilder = new JwtTokenBuilder()
                            .AddSecurityKey(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSetting.SecretKey ?? "")))
                            .AddIssuer(tokenSetting.Issuer ?? "")
                            .AddAudience(tokenSetting.Audience ?? "")
                            .AddExpiry(tokenSetting.ExpirationMinutes ?? 3600);

        if (userToken == null) return tokenBuilder.Build();

        tokenBuilder.AddClaim(JwtRegisteredClaimNames.Jti, userToken.Guid.ToString());

        if (user == null) return tokenBuilder.Build();

        tokenBuilder.AddSubject(user.Guid.ToString());

        return tokenBuilder.Build();
    }

    #region " private "

    private void EnsureArguments()
    {
        if (_securityKey == null)
            throw new ArgumentNullException(nameof(_securityKey));

        if (string.IsNullOrEmpty(_subject))
            throw new ArgumentNullException(nameof(_subject));

        if (string.IsNullOrEmpty(_issuer))
            throw new ArgumentNullException(nameof(_issuer));

        if (string.IsNullOrEmpty(_audience))
            throw new ArgumentNullException(nameof(_audience));
    }

    #endregion
}