using Core.DTOs.LookUps.Category.Response;

namespace Core.DTOs.User.Response
{
    public class OrganizerProfileDto
    {
        public long Id { get; set; }
        public string MapLink { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string Description { get; set; } = string.Empty;
        public CategoryDetailDto Category { get; set; } = new CategoryDetailDto();
    }
}
