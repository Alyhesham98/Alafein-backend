using Core.Entities.BaseEntities;
using Core.Entities.Identity;

namespace Core.Entities.Event
{
    public class BlockedComment : BaseEntity
    {
        public string Reason { get; set; } = string.Empty;

        public long SubmissionCommentId { get; set; }
        public SubmissionComment SubmissionComment { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
