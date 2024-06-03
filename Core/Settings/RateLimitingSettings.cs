namespace Core.Settings
{
    public class RateLimitingSettings
    {
        public const string RateLimitSection = "RateLimitingSettings";
        public string RateLimitMessage { get; set; } = string.Empty;
        public string ThirdPartiesPolicy { get; set; } = string.Empty;
        public int ThirdPartyWindowLimit { get; set; }
        public int ThirdPartyWindowPeriod { get; set; }
        public string DefaultGlobalTokenBucketKey { get; set; } = string.Empty;
        public string GlobalTokenBucketHeaderName { get; set; } = string.Empty;
        public int GlobalTokenReplenishmentPeriod { get; set; }
        public int GlobalTokensLimit { get; set; }
        public int GlobalTokensPerPeriod { get; set; }
    }
}
