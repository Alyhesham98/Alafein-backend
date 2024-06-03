namespace Core.DTOs.User.Response
{
    public class ListEventOrganizerDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public int EventCount { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
