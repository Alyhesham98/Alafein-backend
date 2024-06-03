using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class Role : IdentityRole
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
