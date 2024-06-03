using Core.Entities.BaseEntities;

namespace Core.Entities.Event
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;

        public long VenueId { get; set; }
        public Venue Venue { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
