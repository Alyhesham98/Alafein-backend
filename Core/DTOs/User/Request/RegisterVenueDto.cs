namespace Core.DTOs.User.Request
{
    public class RegisterVenueDto
    {
        public AdminAddUserDto User { get; set; } = new AdminAddUserDto();
        public VenueDto? Venue { get; set; }
    }
}
