using Core;
using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.User.Response;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.Event.Services;
using DTOs.Shared.Responses;

namespace Services.Implementation.Event
{
    internal sealed class CommentService : ICommentService
    {
        private readonly ISubmissionCommentRepository _submissionCommentRepo;
        private readonly IBlockedCommentRepository _blockedCommentRepo;
        private readonly ISubmissionDateRepository _submissionDateRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CommentService(ISubmissionCommentRepository submissionCommentRepo,
                              IBlockedCommentRepository blockedCommentRepo,
                              ISubmissionDateRepository submissionDateRepo,
                              IUnitOfWork unitOfWork)
        {
            _submissionCommentRepo = submissionCommentRepo;
            _blockedCommentRepo = blockedCommentRepo;
            _submissionDateRepo = submissionDateRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> ToggleComment(ToggleCommentDto request)
        {
            var comment = await _submissionCommentRepo.GetByIdAsync(request.Id);
            if (comment is null)
            {
                return new Response<bool>("Comment is not found.");
            }

            if (comment.IsApproved)
            {
                comment.IsApproved = false;
            }
            else
            {
                comment.IsApproved = true;
            }

            _submissionCommentRepo.Update(comment);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<PagedResponse<IList<ListCommentDto>>> GetPagination(CommentListParameters filter)
        {
            var result = await _submissionCommentRepo.GetComments(filter);
            if (result.Count == 0 ||
                result.Data == null)
            {
                return new PagedResponse<IList<ListCommentDto>>(null, filter.PageNumber, filter.PageSize);
            }
            return new PagedResponse<IList<ListCommentDto>>(result.Data, filter.PageNumber, filter.PageSize, result.Count);
        }

        public async Task<Response<IList<EventCommentDto>>> GetEventComments(long id)
        {
            var submissionId = await _submissionDateRepo.GetPropertyWithSelectorAsync(s => s.SubmissionId,
                                                                                      true,
                                                                                      f => f.Id == id);

            var result = await _submissionCommentRepo.GetAllWithSelectorAsync(s => new EventCommentDto
            {
                Comment = s.Comment,
                User = new UserCommentDto
                {
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Photo = s.User.ProfilePicture
                }
            }, true,
            f => f.IsApproved &&
                 f.SubmissionId == submissionId);

            return new Response<IList<EventCommentDto>>(result);
        }
    }
}
