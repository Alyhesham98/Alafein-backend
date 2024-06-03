namespace Core.DTOs.User.Response
{
    public class UserDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string Address { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;
    }
}
