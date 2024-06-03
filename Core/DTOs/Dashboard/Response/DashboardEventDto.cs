using Core.DTOs.User.Response;
using DTOs.Shared;

namespace Core.DTOs.Dashboard.Response
{
    public class DashboardEventDto
    {
        public long Id { get; set; }
        public string Poster { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DropdownViewModel Category { get; set; } = new DropdownViewModel();
        public DropdownViewModel Venue { get; set; } = new DropdownViewModel();
        public string? VenueImage { get; set; }
        public IdentityDropdownDto Organizer { get; set; } = new IdentityDropdownDto();
        public string? OrganizerImage { get; set; }
        public List<string> Date { get; set; } = new List<string>();
    }
}
