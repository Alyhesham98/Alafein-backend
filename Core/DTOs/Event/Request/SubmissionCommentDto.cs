namespace Core.DTOs.Event.Request
{
    public class SubmissionCommentDto
    {
        public string Comment { get; set; } = string.Empty;
        public long SubmissionId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
