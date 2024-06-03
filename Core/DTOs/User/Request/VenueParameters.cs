using Core.DTOs.Shared;

namespace Core.DTOs.User.Request
{
    public class VenueParameters : PaginationParameter
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
