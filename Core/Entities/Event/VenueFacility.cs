using Core.Entities.BaseEntities;
using Core.Entities.LookUps;

namespace Core.Entities.Event
{
    public class VenueFacility : BaseEntity
    {
        public long VenueId { get; set; }
        public Venue Venue { get; set; }

        public long FacilityId { get; set; }
        public Facility Facility { get; set; }
    }
}
