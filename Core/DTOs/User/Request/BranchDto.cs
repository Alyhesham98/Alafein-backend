namespace Core.DTOs.User.Request
{
    public class BranchDto
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;
        public List<WorkDayDto> WorkDays { get; set; } = new List<WorkDayDto>();
    }
}
