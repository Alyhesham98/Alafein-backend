namespace Core.DTOs.User.Request
{
    public class RegisterDto
    {
        public UserAddModel User { get; set; } = new UserAddModel();
        public OrganizerDto? Organizer { get; set; }
        public VenueDto? Venue { get; set; }
    }
}
