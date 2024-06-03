using Core.Entities.BaseEntities;

namespace Core.Entities.Event
{
    public class SubmissionDate : BaseEntity
    {
        public DateTime Date { get; set; }
        public bool IsCancelled { get; set; }
        public int SpotlightOrder { get; set; }

        public long SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
