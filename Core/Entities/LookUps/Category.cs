using Core.Entities.BaseEntities;
using Core.Entities.Event;

namespace Core.Entities.LookUps
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public int SortNo { get; set; }

        public ICollection<Venue> Venues { get; set; }
        public ICollection<Organizer> Organizers { get; set; }
        public ICollection<Submission> Submissions { get; set; }
    }
}
