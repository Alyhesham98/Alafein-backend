namespace Core.Settings
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public int EmailsLimitPerMonth { get; set; }
        public int EmailsLimitPerDay { get; set; }
        public int EmailsLimitPerMinuite { get; set; }
    }
}
