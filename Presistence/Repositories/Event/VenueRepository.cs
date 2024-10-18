using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class VenueRepository : GenericRepository<Venue>, IVenueRepository
    {
        private readonly ApplicationDbContext _context;
        public VenueRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> GetLastTaskOrderId()
        {
            var item = await _context.Venues.OrderBy(u => u.Id).LastOrDefaultAsync();
            return (int)item.Id;
        }

        public async Task<(int Count, IList<ListVenueDto>? Data)> GetPaginaton(VenueParameters parameters)
        {
            var venues = _context.Venues
                                 .Where(f => !f.IsDeleted).OrderByDescending(o => o.Id);

            if (parameters.Name is not null)
            {
                var search = parameters.Name.Trim();

                venues = venues.Where(f => f.User
                                            .FirstName
                                            .Contains(search) ||
                                           f.User
                                            .LastName
                                            .Contains(search)).OrderByDescending(o => o.Id);
            }

            if (parameters.Email is not null)
            {
                var search = parameters.Email.Trim();

                venues = venues.Where(f => f.User
                                            .Email!
                                            .Contains(search)).OrderByDescending(o => o.Id);
            }

            var count = await venues.CountAsync();

            var data = await venues.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                   .Take(parameters.PageSize)
                                   .Select(s => new ListVenueDto
                                   {
                                       Id = s.Id,
                                       UserId = s.UserId,
                                       FirstName = s.User.FirstName,
                                       LastName = s.User.LastName,
                                       Email = s.User.Email!,
                                       Photo = s.User.ProfilePicture,
                                       Facility = s.VenueFacilities
                                                   .Where(f => !f.IsDeleted && f.VenueId == s.Id)
                                                   .Select(sf => new ListFacilityDto
                                                   {
                                                       Id = sf.Id,
                                                       ImageName = sf.Facility.ImageName,
                                                       ImagePath = sf.Facility.ImagePath
                                                   }).ToList(),
                                       IsBlocked = s.User.IsBlocked,
                                       CreatedAt = s.CreatedAt
                                   })
                                   .AsNoTracking()
                                   .ToListAsync();

            return new(count, data);
        }
    }
}
