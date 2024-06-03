namespace Core.DTOs.User.Request
{
    public class OrganizerDto
    {
        public string? MapLink { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string Description { get; set; } = string.Empty;

        public long CategoryId { get; set; }
    }
}
