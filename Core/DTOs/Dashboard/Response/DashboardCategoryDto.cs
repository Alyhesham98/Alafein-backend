namespace Core.DTOs.Dashboard.Response
{
    public class DashboardCategoryDto
    {
        public long Id { get; set; }
        public string Photo { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int NumberOfEvent { get; set; }
    }
}
