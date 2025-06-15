namespace RecipeShareLibrary.Model.Settings;

public class RateLimiterSettings
{
    public const string MyRateLimit = "fixed";
    public int PermitLimit { get; set; } = 10;
    public int Window { get; set; } = 60;
    public int QueueLimit { get; set; } = 0;
}