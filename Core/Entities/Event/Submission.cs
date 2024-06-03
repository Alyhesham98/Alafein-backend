using Core.Entities.BaseEntities;
using Core.Entities.Identity;
using Core.Entities.LookUps;
using Core.Enums;

namespace Core.Entities.Event
{
    public class Submission : BaseEntity
    {
        public string EventNameEN { get; set; } = string.Empty;
        public string EventNameAR { get; set; } = string.Empty;
        public string EventDescriptionEN { get; set; } = string.Empty;
        public string EventDescriptionAR { get; set; } = string.Empty;
        public string? MainArtestNameEN { get; set; }
        public string? MainArtestNameAR { get; set; }
        public bool KidsAvailability { get; set; }
        public AttendanceOption AttendanceOption { get; set; }
        public string? URL { get; set; }
        public decimal PaymentFee { get; set; }
        public string? Poster { get; set; }
        public string? ContactPerson { get; set; }
        public string? AddtionalComment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSpotlight { get; set; }
        public SubmissionStatus Status { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public long VenueId { get; set; }
        public Venue Venue { get; set; }
        public long BranchId { get; set; }
        public Branch Branch { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
        public ICollection<SubmissionDate> Dates { get; set; }
        public ICollection<FavouriteSubmission> FavouriteSubmissions { get; set; }
        public ICollection<SubmissionComment> SubmissionComments { get; set; }
    }
}
