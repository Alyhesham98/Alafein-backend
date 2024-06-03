using Core.Entities.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class UserOTP : BaseEntity
    {
        [MaxLength(6)]
        public string OTP { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
