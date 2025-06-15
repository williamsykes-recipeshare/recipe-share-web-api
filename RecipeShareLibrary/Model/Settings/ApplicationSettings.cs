namespace RecipeShareLibrary.Model.Settings;

public class ApplicationSettings
{
    public string[]? AuthorisedDomains { get; set; }
    public long? SlowResponseLogThreshold { get; set; }
    public TokenSettings? TokenSettings { get; set; }
    public RateLimiterSettings? RateLimiterSettings { get; set; }
}