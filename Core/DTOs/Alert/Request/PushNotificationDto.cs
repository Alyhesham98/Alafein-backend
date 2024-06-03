namespace Core.DTOs.Alert.Request
{
    public class PushNotificationDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    }
}
