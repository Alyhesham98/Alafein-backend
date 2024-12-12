using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Enums;
using Core.Interfaces.Event.Repositories;
using DTOs.Shared;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;
using System.Linq.Expressions;

namespace Presistence.Repositories.Event
{
    internal sealed class SubmissionDateRepository : GenericRepository<SubmissionDate>, ISubmissionDateRepository
    {
        private readonly ApplicationDbContext _context;
        public SubmissionDateRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(int Count, IList<ListEventDto>? Data)> GetEvents(EventListParameters parameters, Expression<Func<SubmissionDate, bool>>? filter)
        {
            var events = _context.SubmissionDates
                                 .Where(f => !f.IsDeleted).OrderBy(o => o.Date.Day);

            if (filter is not null)
            {
                events = events.Where(filter).OrderBy(o => o.Date.Day);
            }

            if (parameters.Name is not null)
            {
                var search = parameters.Name
                                       .Trim();


                events = events.Where(f => f.Submission
                                            .EventNameAR
                                            .Contains(search)).OrderBy(o => o.Date.Day);
                events = events.Where(f => f.Submission
                            .EventNameEN
                            .Contains(search)).OrderBy(o => o.Date.Day);
            }
            if (parameters.Venue is not null)
            {
                var search = parameters.Venue.Trim();

                events = events.Where(f => f.Submission
                                            .Venue
                                            .User
                                            .FirstName
                                            .Contains(search) ||
                                           f.Submission
                                            .Venue
                                            .User
                                            .LastName
                                            .Contains(search)).OrderBy(o => o.Date.Day);
            }

            if (parameters.Organizer is not null)
            {
                var search = parameters.Organizer.Trim();

                events = events.Where(f => f.Submission
                                            .User
                                            .FirstName
                                            .Contains(search) ||
                                           f.Submission
                                            .User
                                            .LastName
                                            .Contains(search)).OrderBy(o => o.Date.Day);
            }

            if (parameters.CategoryId is not null)
            {
                events = events.Where(f => f.Submission
                                            .CategoryId == parameters.CategoryId).OrderBy(o => o.Date.Day);
            }

            if (parameters.IsApproved is not null)
            {
                events = events.Where(f => f.Submission
                                            .IsApproved == parameters.IsApproved).OrderBy(o => o.Date.Day);
            }

            if (parameters.IsSpotlight is not null)
            {
                events = events.Where(f => f.Submission
                                            .IsSpotlight == parameters.IsSpotlight).OrderBy(o => o.Date.Day);
            }

            if (parameters.IsPending is not null)
            {
                events = events.Where(f => f.Submission
                                            .Status == SubmissionStatus.PENDING).OrderBy(o => o.Date.Day);
            }

            var count = await events.CountAsync();

            var data = await events.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                   .Take(parameters.PageSize).OrderBy(o => o.Date.Day)
                                   .Select(s => new ListEventDto
                                   {
                                       Id = s.Id,
                                       SubmissionId = s.SubmissionId,
                                       Poster = s.Submission.Poster,
                                       NameEn = s.Submission.EventNameEN,
                                       NameAr = s.Submission.EventNameAR,
                                       Category = new DropdownViewModel
                                       {
                                           Id = s.Submission.Category.Id,
                                           Name = s.Submission.Category.Name
                                       },
                                       Venue = new DropdownViewModel
                                       {
                                           Id = s.Submission.Venue.Id,
                                           Name = s.Submission.Venue.VenueName
                                       },
                                       VenueImage = s.Submission.Venue.User.ProfilePicture,
                                       Organizer = new IdentityDropdownDto
                                       {
                                           Id = s.Submission.User.Id,
                                           Name = s.Submission.User.FirstName + " " + s.Submission.User.LastName
                                       },
                                       OrganizerImage = s.Submission.User.ProfilePicture,
                                       Date = s.Date.ToString("dd MMM, hh:mm tt"),
                                       IsSpotlight = s.Submission.IsSpotlight,
                                       SpotlightOrder = s.SpotlightOrder,
                                       Status = new DropdownViewModel
                                       {
                                           Id = (long)s.Submission.Status,
                                           Name = s.Submission.Status.ToString()
                                       },
                                       CreatedAt = s.CreatedAt
                                   })
                                   .AsNoTracking()
                                   .ToListAsync();

            return new(count, data);
        }

        public async Task<(int Count, IList<ListEventMobileDto>? Data)> GetEventsMobile(EventMobileListParameters parameters, Expression<Func<SubmissionDate, bool>>? filter, string userId)
        {
            var events = _context.SubmissionDates
                                 .Where(f => !f.IsDeleted && f.Submission.IsApproved && f.Submission.Status == SubmissionStatus.ACCEPT).OrderBy(o => o.Date);

            if (filter is not null)
            {
                events = events.Where(filter).OrderBy(o => o.Date);
            }

            if (parameters.Name is not null)
            {
                var search = parameters.Name
                                       .Trim();

                events = events.Where(f => f.Submission
                                            .EventNameAR
                                            .Contains(search)).OrderBy(o => o.Date);
                events = events.Where(f => f.Submission
                            .EventNameEN
                            .Contains(search)).OrderBy(o => o.Date);
            }

            if (parameters.IsFavourite is not null &&
                parameters.IsFavourite == true)
            {
                events = events.Where(f => f.Submission
                                            .FavouriteSubmissions
                                            .Any(c => c.SubmissionId == f.SubmissionId &&
                                                      c.UserId == userId)).OrderBy(o => o.Date);
            }

            if (parameters.From is not null)
            {
                events = events.Where(f => f.Date.Day >= parameters.From.Value.Day).OrderBy(o => o.Date);
            }

            if (parameters.To is not null)
            {
                events = events.Where(f => f.Date.Day <= parameters.To.Value.Day).OrderBy(o => o.Date);
            }

            if (parameters.CategoryId is not null)
            {
                events = events.Where(f => f.Submission.CategoryId == parameters.CategoryId).OrderBy(o => o.Date);
            }

            if (parameters.MinFee is not null)
            {
                events = events.Where(f => f.Submission.PaymentFee >= parameters.MinFee).OrderBy(o => o.Date);
            }

            if (parameters.MaxFee is not null)
            {
                events = events.Where(f => f.Submission.PaymentFee <= parameters.MaxFee).OrderBy(o => o.Date);
            }

            var count = await events.CountAsync();

            var data = await events.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                   .Take(parameters.PageSize).OrderBy(o => o.Date)
                                   .Select(s => new ListEventMobileDto
                                   {
                                       Id = s.Id,
                                       Poster = s.Submission.Poster,
                                       NameEn = s.Submission.EventNameEN,
                                       NameAr = s.Submission.EventNameAR,
                                       Venue = new DropdownViewModel
                                       {
                                           Id = s.Submission.Venue.Id,
                                           Name = s.Submission.Venue.VenueName
                                       },
                                       Date = s.Date.ToString("dd MMM, hh:mm tt"),
                                       IsFavourite = s.Submission
                                                      .FavouriteSubmissions
                                                      .Any(c => c.SubmissionId == s.SubmissionId &&
                                                                c.UserId == userId)
                                   })
                                   .AsNoTracking()
                                   .ToListAsync();

            return new(count, data);
        }

        public async Task<IList<HomeEventDto>> Home(HomeEventListParameters parameters, Expression<Func<SubmissionDate, bool>>? filter)
        {
            var events = _context.SubmissionDates
                                 .Where(f => !f.IsDeleted && f.Submission.IsApproved);

            if (filter is not null)
            {
                events = events.Where(filter);
            }

            if (parameters.VenueId is not null)
            {
                events = events.Where(f => f.Submission
                                            .VenueId == parameters.VenueId);
            }

            if (parameters.OrganizerId is not null)
            {
                events = events.Where(f => f.Submission
                                            .UserId == parameters.OrganizerId);
            }

            if (parameters.IsSpotlight is not null)
            {
                events = events.Where(f => f.Submission
                                            .IsSpotlight == parameters.IsSpotlight);
            }

            if (parameters.OrderSpotlight)
            {
                events = events.OrderBy(o => o.SpotlightOrder);
            }
            else
            {
                events = events.OrderBy(o => o.Id);
            }

            var data = await events.Select(s => new HomeEventDto
            {
                Id = s.Id,
                Poster = s.Submission.Poster,
                Name = s.Submission.EventNameEN,
                Date = s.Date.ToString("dd MMM, hh:mm tt"),
                CategoryPoster = s.Submission.Category.Image
            }).AsNoTracking()
            .ToListAsync();

            return data;
        }

        public Task<List<SubmissionDate>> GetBySubmissionId(long? submissionId)
        {
            var events = _context.SubmissionDates
                                 .Where(f => !f.IsDeleted && f.SubmissionId == submissionId).ToListAsync();
            return events;
        }

        public async Task<bool> SpotlightOrder(long id, int order)
        {
            await _context.SubmissionDates
                          .Where(x => x.Id == id)
                          .ExecuteUpdateAsync(e => e.SetProperty(d => d.SpotlightOrder, order));
            return true;
        }

        public async Task<bool> DeleteSubmissionDates(long submissionId)
        {
            await _context.SubmissionDates
                          .Where(x => x.SubmissionId == submissionId)
                          .ExecuteUpdateAsync(e => e.SetProperty(d => d.IsDeleted, true));
            return true;
        }
    }
}
