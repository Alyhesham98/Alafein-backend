using Core.Entities.BaseEntities;
using Core.Entities.Identity;
using Core.Entities.LookUps;

namespace Core.Entities.Event
{
    public class Organizer : BaseEntity
    {
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string? MapLink { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
