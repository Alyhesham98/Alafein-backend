using Core.Entities.BaseEntities;
using Core.Entities.Identity;

namespace Core.Entities.Alert
{
    public class NotificationHistory : BaseEntity
    {
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }

        public long NotificationId { get; set; }
        public Notification Notification { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
