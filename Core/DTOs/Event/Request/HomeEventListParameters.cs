namespace Core.DTOs.Event.Request
{
    public class HomeEventListParameters
    {
        public long? VenueId { get; set; }
        public string? OrganizerId { get; set; }
        public bool? IsSpotlight { get; set; }
        public bool OrderSpotlight { get; set; }
    }
}
