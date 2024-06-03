namespace Core.Settings
{
    public class MailSettings
    {
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string VerifyAccountPath { get; set; } = string.Empty;
        public string ForgetPath { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
    }
}
