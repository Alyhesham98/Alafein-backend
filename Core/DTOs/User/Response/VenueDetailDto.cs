using Core.DTOs.Event.Response;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.LookUps.Facility.Response;

namespace Core.DTOs.User.Response
{
    public class VenueDetailDto
    {
        public UserDetailViewModel User { get; set; } = new UserDetailViewModel();
        public long Id { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string VenueDescription { get; set; } = string.Empty;
        public CategoryDetailDto Category { get; set; } = new CategoryDetailDto();
        public List<string> Photos { get; set; } = new List<string>();
        public List<FacilityDetailDto> Facilities { get; set; } = new List<FacilityDetailDto>();
        public List<BranchDetailDto> Branches { get; set; } = new List<BranchDetailDto>();
    }
}
