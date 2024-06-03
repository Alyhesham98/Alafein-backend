namespace Core.DTOs.User.Request
{
    public class UserDeviceNotificationDto
    {
        public string NotificationToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
