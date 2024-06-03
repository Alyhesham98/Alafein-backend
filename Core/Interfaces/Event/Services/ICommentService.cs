using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Event.Services
{
    public interface ICommentService
    {
        Task<Response<bool>> ToggleComment(ToggleCommentDto request);
        Task<PagedResponse<IList<ListCommentDto>>> GetPagination(CommentListParameters filter);
        Task<Response<IList<EventCommentDto>>> GetEventComments(long id);
    }
}
