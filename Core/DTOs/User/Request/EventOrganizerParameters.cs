using Core.DTOs.Shared;

namespace Core.DTOs.User.Request
{
    public class EventOrganizerParameters : PaginationParameter
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
