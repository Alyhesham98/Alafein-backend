namespace Core.DTOs.User.Request
{
    public class MobileLoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserDeviceDto? Device { get; set; }
    }
}
