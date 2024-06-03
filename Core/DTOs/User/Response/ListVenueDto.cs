using Core.DTOs.LookUps.Facility.Response;

namespace Core.DTOs.User.Response
{
    public class ListVenueDto
    {
        public long Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public bool IsBlocked { get; set; }
        public List<ListFacilityDto> Facility { get; set; } = new List<ListFacilityDto>();
        public DateTime CreatedAt { get; set; }
    }
}
