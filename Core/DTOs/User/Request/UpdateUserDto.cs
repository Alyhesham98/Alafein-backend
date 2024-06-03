namespace Core.DTOs.User.Request
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? Phone { get; set; }
    }
}
