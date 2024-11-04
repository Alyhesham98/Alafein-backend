using Core.DTOs.Shared;

namespace Core.DTOs.Event.Request
{
    public class EventListParameters : PaginationParameter
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Venue { get; set; }
        public string? Organizer { get; set; }
        public long? CategoryId { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsSpotlight { get; set; }
        public bool? IsPending { get; set; }
    }
}
