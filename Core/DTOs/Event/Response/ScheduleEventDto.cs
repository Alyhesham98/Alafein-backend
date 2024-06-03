namespace Core.DTOs.Event.Response
{
    public class ScheduleEventDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public List<string> Date { get; set; } = new List<string>();
        public string CategoryPoster { get; set; } = string.Empty;
    }
}
