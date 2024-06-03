using Core.Entities.BaseEntities;

namespace Core.Entities.Event
{
    public class Photo : BaseEntity
    {
        public string Image { get; set; } = string.Empty;

        public long VenueId { get; set; }
        public Venue Venue { get; set; }
    }
}
