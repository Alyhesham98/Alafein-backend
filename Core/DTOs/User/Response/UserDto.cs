using DTOs.Shared;

namespace Core.DTOs.User.Response
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public DropdownViewModel Status { get; set; } = new DropdownViewModel();
        public bool IsBlocked { get; set; }
    }
}
