using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class VenueDropdownDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DropdownViewModel> Branch { get; set; } = new List<DropdownViewModel>();
    }
}
