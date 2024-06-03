namespace Core.DTOs.Alert.Response
{
    public class ListNotificationDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string BodyAr { get; set; } = string.Empty;
        public DateTime? Schedule { get; set; }
    }
}
