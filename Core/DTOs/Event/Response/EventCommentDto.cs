using Core.DTOs.User.Response;

namespace Core.DTOs.Event.Response
{
    public class EventCommentDto
    {
        public string Comment { get; set; } = string.Empty;
        public UserCommentDto User { get; set; } = new UserCommentDto();
    }
}
