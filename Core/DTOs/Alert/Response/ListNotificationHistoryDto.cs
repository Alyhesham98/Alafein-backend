namespace Core.DTOs.Alert.Response
{
    public class ListNotificationHistoryDto
    {
        public long Id { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
