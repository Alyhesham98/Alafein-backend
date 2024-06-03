using Core.DTOs.LookUps.Category.Response;

namespace Core.DTOs.Event.Response
{
    public class HomeDto
    {
        public IList<HomeEventDto>? Spotlight { get; set; }
        public IList<CategoryDropdownDto> Category { get; set; } = new List<CategoryDropdownDto>();
        public IList<HomeEventDto>? Today { get; set; }
    }
}
