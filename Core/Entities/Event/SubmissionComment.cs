using Core.Entities.BaseEntities;
using Core.Entities.Identity;

namespace Core.Entities.Event
{
    public class SubmissionComment : BaseEntity
    {
        public string Comment { get; set; } = string.Empty;
        public bool IsApproved { get; set; }

        public long SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public ICollection<BlockedComment> BlockedComments { get; set; }
    }
}
