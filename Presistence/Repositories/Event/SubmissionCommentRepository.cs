using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using DTOs.Shared;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class SubmissionCommentRepository : GenericRepository<SubmissionComment>, ISubmissionCommentRepository
    {
        private readonly ApplicationDbContext _context;
        public SubmissionCommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(int Count, IList<ListCommentDto>? Data)> GetComments(CommentListParameters parameters)
        {
            var comments = _context.SubmissionComments
                                   .Where(f => !f.IsDeleted &&
                                               !f.IsApproved).OrderByDescending(x=>x.Id);

            if (parameters.Event is not null)
            {
                var search = parameters.Event
                                       .Trim();

                comments = comments.Where(f => f.Submission
                                                .EventNameEN
                                                .Contains(search)).OrderByDescending(x => x.Id);
            }

            if (parameters.Name is not null)
            {
                var search = parameters.Name.Trim();

                comments = comments.Where(f => f.User
                                                .FirstName
                                                .Contains(search) ||
                                               f.User
                                                .LastName
                                                .Contains(search)).OrderByDescending(x => x.Id);
            }

            var count = await comments.CountAsync();

            var data = await comments.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                     .Take(parameters.PageSize)
                                     .Select(s => new ListCommentDto
                                     {
                                         Id = s.Id,
                                         User = new IdentityDropdownDto
                                         {
                                             Id = s.User.Id,
                                             Name = s.User.FirstName + " " + s.User.LastName
                                         },
                                         ProfilePicture = s.User.ProfilePicture,
                                         Comment = s.Comment,
                                         Event = new DropdownViewModel
                                         {
                                             Id = s.Submission.Id,
                                             Name = s.Submission.EventNameEN
                                         },
                                         Poster = s.Submission.Poster
                                     })
                                     .AsNoTracking()
                                     .ToListAsync();

            return new(count, data);
        }
    }
}
