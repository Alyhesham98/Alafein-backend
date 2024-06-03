using Core.Entities.BaseEntities;
using Core.Entities.Identity;
using Core.Entities.LookUps;

namespace Core.Entities.Event
{
    public class Venue : BaseEntity
    {
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string VenueDescription { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<VenueFacility> VenueFacilities { get; set; }
        public ICollection<Branch> Branches { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
