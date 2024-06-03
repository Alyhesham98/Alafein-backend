namespace Core.DTOs.User.Response
{
    public class ListAdminDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public bool IsBlocked { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
