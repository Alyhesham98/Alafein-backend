using Core.DTOs.User.Response;
using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class ListEventDto
    {
        public long Id { get; set; }
        public long SubmissionId { get; set; }
        public string Poster { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public DropdownViewModel Category { get; set; } = new DropdownViewModel();
        public DropdownViewModel Venue { get; set; } = new DropdownViewModel();
        public string? VenueImage { get; set; }
        public IdentityDropdownDto Organizer { get; set; } = new IdentityDropdownDto();
        public string? OrganizerImage { get; set; }
        public string Date { get; set; } = string.Empty;
        public bool IsSpotlight { get; set; }
        public int SpotlightOrder { get; set; }
        public DropdownViewModel Status { get; set; } = new DropdownViewModel();
        public DateTime CreatedAt { get; set; }
    }
}
