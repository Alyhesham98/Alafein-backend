using Core.DTOs.Shared;

namespace Core.DTOs.Event.Request
{
    public class CommentListParameters : PaginationParameter
    {
        public string? Name { get; set; }
        public string? Event { get; set; }
    }
}
