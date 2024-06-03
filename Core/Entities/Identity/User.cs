using Core.Entities.Alert;
using Core.Entities.Event;
using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public Status Status { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsBlockedFromComment { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public virtual DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public UserDevice UserDevice { get; set; }
        public ICollection<UserOTP> UserOtps { get; set; }
        public ICollection<Venue> Venues { get; set; }
        public ICollection<Organizer> Organizers { get; set; }
        public ICollection<Submission> Submissions { get; set; }
        public ICollection<FavouriteSubmission> FavouriteSubmissions { get; set; }
        public ICollection<SubmissionComment> SubmissionComments { get; set; }
        public ICollection<BlockedComment> BlockedComments { get; set; }
        public ICollection<NotificationHistory> NotificationHistory { get; set; }
    }
}
