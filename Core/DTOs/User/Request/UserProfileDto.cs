namespace Core.DTOs.User.Request
{
    public class UserProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? Phone { get; set; }
    }
}
