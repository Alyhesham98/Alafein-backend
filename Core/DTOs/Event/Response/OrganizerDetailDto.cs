using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.User.Response;

namespace Core.DTOs.Event.Response
{
    public class OrganizerDetailDto
    {
        public UserDetailViewModel User { get; set; } = new UserDetailViewModel();
        public long Id { get; set; }
        public string? MapLink { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public CategoryDetailDto Category { get; set; } = new CategoryDetailDto();
    }
}
