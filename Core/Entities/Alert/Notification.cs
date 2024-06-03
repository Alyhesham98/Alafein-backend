using Core.Entities.BaseEntities;

namespace Core.Entities.Alert
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string BodyAr { get; set; } = string.Empty;
        public DateTime? Schedule { get; set; }
        public string? CustomColor { get; set; }
        public string? CustomTextColor { get; set; }
        public string? CustomImageUrl { get; set; }

        public ICollection<NotificationHistory> NotificationHistory { get; set; }
    }
}
