namespace Core.DTOs.Event.Response
{
    public class HomeEventDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string CategoryPoster { get; set; } = string.Empty;
    }
}
