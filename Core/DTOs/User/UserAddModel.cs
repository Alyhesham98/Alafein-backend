using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.User
{
    public class UserAddModel : AdminAddUserDto
    {
        [Required]
        public string RoleId { get; set; } = string.Empty;
    }
}
