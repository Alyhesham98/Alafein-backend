namespace Core.DTOs.User.Request
{
    public class ExternalLoginViewModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public UserDeviceDto? Device { get; set; }
    }
}
