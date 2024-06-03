using Core.Enums;

namespace Core.DTOs.User.Request
{
    public class ToggleStatusDto
    {
        public string Id { get; set; } = string.Empty;
        public Status Status { get; set; }
    }
}
