namespace Core.DTOs.User.Request
{
    public class VenueDto
    {
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? WebsiteURL { get; set; }
        public string? Other { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string VenueDescription { get; set; } = string.Empty;
        public long CategoryId { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
        public List<long> VenueFacilities { get; set; } = new List<long>();
        public List<BranchDto> Branches { get; set; } = new List<BranchDto>();
    }
}
