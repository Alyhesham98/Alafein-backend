using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.User.Request
{
    public class SendOtpDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
