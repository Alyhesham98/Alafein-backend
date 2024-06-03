using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.Entities.Event;
using Core.Interfaces.Base;

namespace Core.Interfaces.Event.Repositories
{
    public interface ISubmissionCommentRepository : IGenericRepository<SubmissionComment>
    {
        Task<(int Count, IList<ListCommentDto>? Data)> GetComments(CommentListParameters parameters);
    }
}
