using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.User
{
    public class AdminAddUserDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }

    }
}
