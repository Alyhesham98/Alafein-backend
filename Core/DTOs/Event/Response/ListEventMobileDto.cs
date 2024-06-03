using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class ListEventMobileDto
    {
        public long Id { get; set; }
        public string? Poster { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public DropdownViewModel Venue { get; set; } = new DropdownViewModel();
        public string Date { get; set; } = string.Empty;
        public bool? IsFavourite { get; set; }
    }
}
