using Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Event
{
    public class FavouriteSubmission
    {
        [Key, Column(Order = 1)]
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        [Key, Column(Order = 2)]
        public long SubmissionId { get; set; }
        public Submission Submission { get; set; }
    }
}
