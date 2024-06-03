namespace Core.DTOs.User.Request
{
    public class WorkDayDto
    {
        public DayOfWeek Day { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
    }
}
