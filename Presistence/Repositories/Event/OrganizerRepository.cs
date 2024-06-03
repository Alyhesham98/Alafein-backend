using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Enums;
using Core.Interfaces.Event.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class OrganizerRepository : GenericRepository<Organizer>, IOrganizerRepository
    {
        private readonly ApplicationDbContext _context;
        public OrganizerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(int Count, IList<ListEventOrganizerDto>? Data)> GetPaginaton(EventOrganizerParameters parameters)
        {
            var events = _context.Organizers
                                 .Where(f => !f.IsDeleted).OrderByDescending(o => o.Id);

            if (parameters.Name is not null)
            {
                var search = parameters.Name.Trim();

                events = events.Where(f => f.User
                                            .FirstName
                                            .Contains(search) ||
                                           f.User
                                            .LastName
                                            .Contains(search)).OrderByDescending(o => o.Id);
            }

            if (parameters.Email is not null)
            {
                var search = parameters.Email.Trim();

                events = events.Where(f => f.User
                                            .Email!
                                            .Contains(search)).OrderByDescending(o => o.Id);
            }
            var count = await events.CountAsync();

            var data = await events.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                   .Take(parameters.PageSize)
                                   .Select(s => new ListEventOrganizerDto
                                   {
                                       Id = s.Id,
                                       UserId = s.UserId,
                                       Facebook = s.Facebook,
                                       Instagram = s.Instagram,
                                       WebsiteURL = s.WebsiteURL,
                                       FirstName = s.User.FirstName,
                                       LastName = s.User.LastName,
                                       Email = s.User.Email!,
                                       Photo = s.User.ProfilePicture,
                                       EventCount = s.User
                                                     .Submissions
                                                     .Where(f => f.UserId == s.UserId &&
                                                                 f.Status == SubmissionStatus.ACCEPT)
                                                     .Count(),
                                       IsBlocked = s.User.IsBlocked,
                                       CreatedAt =s.CreatedAt
                                   })
                                   .AsNoTracking()
                                   .ToListAsync();

            return new(count, data);
        }
    }
}
