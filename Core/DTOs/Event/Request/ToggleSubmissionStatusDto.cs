using Core.Enums;

namespace Core.DTOs.Event.Request
{
    public class ToggleSubmissionStatusDto
    {
        public long Id { get; set; }
        public SubmissionStatus Status { get; set; }
    }
}
