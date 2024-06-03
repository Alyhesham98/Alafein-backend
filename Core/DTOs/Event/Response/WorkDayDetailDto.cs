using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class WorkDayDetailDto
    {
        public long Id { get; set; }
        public DropdownViewModel Day { get; set; } = new DropdownViewModel();
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
    }
}
