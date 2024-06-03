namespace Core.DTOs.User.Request
{
    public class EditOrganizerDto
    {
        public long Id { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;
        public long CategoryId { get; set; }
    }
}
