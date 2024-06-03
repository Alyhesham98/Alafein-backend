namespace Core.DTOs.User.Request
{
    public class PhotoDto
    {
        public string Image { get; set; } = string.Empty;
        public long VenueId { get; set; }
    }
}
