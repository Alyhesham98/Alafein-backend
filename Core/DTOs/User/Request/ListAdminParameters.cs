using Core.DTOs.Shared;

namespace Core.DTOs.User.Request
{
    public class ListAdminParameters : PaginationParameter
    {
        public string? Search { get; set; }
        public string? RoleFilter { get; set; }
    }
}
