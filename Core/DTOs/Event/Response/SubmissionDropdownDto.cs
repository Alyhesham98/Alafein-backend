using Core.DTOs.LookUps.Category.Response;
using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class SubmissionDropdownDto
    {
        public IList<CategoryDropdownDto> Category { get; set; } = new List<CategoryDropdownDto>();
        public IList<VenueDropdownDto> Venue { get; set; } = new List<VenueDropdownDto>();
        public IList<DropdownViewModel> Organizer { get; set; } = new List<DropdownViewModel>();
        public List<RepeatDropdownDto> Repeat { get; set; } = new List<RepeatDropdownDto>();
        public List<DropdownViewModel> Attendance { get; set; } = new List<DropdownViewModel>();
    }
}
