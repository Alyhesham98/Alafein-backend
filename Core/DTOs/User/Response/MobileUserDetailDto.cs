namespace Core.DTOs.User.Response
{
    public class MobileUserDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public string? Phone { get; set; }
        public VenueProfileDto? Venue { get; set; }
        public OrganizerProfileDto? Organizer { get; set; }
    }
}
