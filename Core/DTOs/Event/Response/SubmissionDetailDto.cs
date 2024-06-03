using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.User.Response;
using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class SubmissionDetailDto
    {
        public long Id { get; set; }
        public string EventNameEN { get; set; } = string.Empty;
        public string EventNameAR { get; set; } = string.Empty;
        public string EventDescriptionEN { get; set; } = string.Empty;
        public string EventDescriptionAR { get; set; } = string.Empty;
        public string MainArtestNameEN { get; set; } = string.Empty;
        public string MainArtestNameAR { get; set; } = string.Empty;
        public bool KidsAvailability { get; set; }
        public DropdownViewModel AttendanceOption { get; set; } = new DropdownViewModel();
        public string URL { get; set; } = string.Empty;
        public decimal PaymentFee { get; set; }
        public string Poster { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string AddtionalComment { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public bool IsSpotlight { get; set; }
        public DropdownViewModel Status { get; set; } = new DropdownViewModel();
        public CategoryDropdownDto Category { get; set; } = new CategoryDropdownDto();
        public UserDetailDto Venue { get; set; } = new UserDetailDto();
        public DropdownViewModel Branch { get; set; } = new DropdownViewModel();
        public UserDetailDto Organizer { get; set; } = new UserDetailDto();
        public IdentityDropdownDto EventOrganizer { get; set; } = new IdentityDropdownDto();
        public List<string>? Date { get; set; }
    }
}
