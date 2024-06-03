using Core.Entities.BaseEntities;

namespace Core.Entities.Identity
{
    public class UserDevice : BaseEntity
    {
        public string NotificationToken { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
