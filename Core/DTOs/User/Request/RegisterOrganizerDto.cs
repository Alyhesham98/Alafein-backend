namespace Core.DTOs.User.Request
{
    public class RegisterOrganizerDto
    {
        public AdminAddUserDto User { get; set; } = new AdminAddUserDto();
        public OrganizerDto? Organizer { get; set; }
    }
}
