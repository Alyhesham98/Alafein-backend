namespace Core.DTOs.Dashboard.Response
{
    public class DashboardOrganizerDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public int NumberOfEvent { get; set; }
    }
}
