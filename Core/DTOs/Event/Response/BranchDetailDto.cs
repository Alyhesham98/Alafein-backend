namespace Core.DTOs.Event.Response
{
    public class BranchDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;
        public List<WorkDayDetailDto> WorkDay { get; set; } = new List<WorkDayDetailDto>();
    }
}
