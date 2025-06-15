namespace RecipeShareLibrary.Model.Settings;

public class TokenSettings
{
    public int? ExpirationMinutes { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SecretKey { get; set; }
}