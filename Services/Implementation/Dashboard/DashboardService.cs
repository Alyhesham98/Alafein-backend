using Core.DTOs.Dashboard.Request;
using Core.DTOs.Dashboard.Response;
using Core.DTOs.User.Response;
using Core.Entities.Identity;
using Core.Enums;
using Core.Extensions;
using Core.Interfaces.Dashboard.Services;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.Shared.Services;
using Dapper;
using DTOs.Shared;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementation.Dashboard
{
    internal sealed class DashboardService : IDashboardService
    {
        private readonly ISubmissionRepository _submissionRepo;
        private readonly IOrganizerRepository _organizerRepo;
        private readonly IVenueRepository _venueRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IDateTimeService _dateService;
        private readonly UserManager<User> _userManager;
        public DashboardService(ISubmissionRepository submissionRepo,
                                IOrganizerRepository organizerRepo,
                                IVenueRepository venueRepo,
                                ICategoryRepository categoryRepo,
                                IDateTimeService dateService,
                                UserManager<User> userManager)
        {
            _categoryRepo = categoryRepo;
            _submissionRepo = submissionRepo;
            _organizerRepo = organizerRepo;
            _venueRepo = venueRepo;
            _dateService = dateService;
            _userManager = userManager;
        }

        public async Task<Response<DashboardDto>> Dashboard(DashboardListParameters filter)
        {
            int eventCount = 0; int organizerCount = 0; int venueCount = 0; int userCount = 0;
            #region Filter Count
            if (filter.Count is null ||
                filter.Count == DashboardOption.Week)
            {
                eventCount = await _submissionRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                      f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7));

                organizerCount = await _organizerRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                         f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7));

                venueCount = await _venueRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                 f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7));

                userCount = await _userManager.Users
                                              .Where(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                          f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7))
                                              .CountAsync();
            }
            else if (filter.Count == DashboardOption.Month)
            {
                eventCount = await _submissionRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                      f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1));

                organizerCount = await _organizerRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                         f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1));

                venueCount = await _venueRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                 f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1));

                userCount = await _userManager.Users
                                              .Where(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                          f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1))
                                              .CountAsync();
            }
            else
            {
                eventCount = await _submissionRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                      f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1));

                organizerCount = await _organizerRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                         f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1));

                venueCount = await _venueRepo.GetCountAsync(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                                 f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1));

                userCount = await _userManager.Users
                                              .Where(f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                                                          f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1))
                                              .CountAsync();
            }
            #endregion


            #region Filter Categories
            var categories = await _categoryRepo.GetPagedWithSelectorAsync(s => new DashboardCategoryDto
            {
                Id = s.Id,
                Name = s.Name,
                Photo = s.Image,
                NumberOfEvent = s.Submissions
                                 .Where(sf => sf.CategoryId == s.Id)
                                 .Count()
            },
            1,
            3,
            true,
            f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                 (filter.Categories == null ||
                  filter.Categories == DashboardOption.Week) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7) :
                                                               (filter.Categories == DashboardOption.Month) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1) :
                                                                                                              f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1),
            o => o.OrderByDescending(x => x.Id));
            #endregion


            #region Filter Venues
            var venues = await _venueRepo.GetPagedWithSelectorAsync(s => new DashboardVenueDto
            {
                Id = s.Id,
                Name = s.VenueName,
                Photo = s.User.ProfilePicture,
                NumberOfEvent = s.Submissions
                                .Where(sf => sf.VenueId == s.Id)
                                .Count()
            },
            1,
            3,
            true,
            f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                 (filter.Venues == null ||
                  filter.Venues == DashboardOption.Week) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7) :
                                                           (filter.Venues == DashboardOption.Month) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1) :
                                                                                                              f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1),
            o => o.OrderByDescending(x => x.Id));
            #endregion


            #region Filter Organizer
            var organizer = await _organizerRepo.GetPagedWithSelectorAsync(s => new DashboardOrganizerDto
            {
                Id = s.Id,
                Name = s.User.FirstName + " " + s.User.LastName,
                Photo = s.User.ProfilePicture,
                NumberOfEvent = s.User.Submissions.Count
            },
            1,
            3,
            true,
            f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                 (filter.Organizer == null ||
                  filter.Organizer == DashboardOption.Week) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7) :
                                                              (filter.Organizer == DashboardOption.Month) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1) :
                                                                                                              f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1),
            o => o.OrderByDescending(x => x.Id));
            #endregion


            #region Filter Top Audience         
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Search", filter.TopUsers);
            parameters.Add("@PageNumber", 1);
            parameters.Add("@PageSize", 3);

            var topUsers = await _categoryRepo.QueryAsync<TopUserDto>("[dbo].[SP_GetAudienceDashboard]", parameters, System.Data.CommandType.StoredProcedure);
            #endregion


            #region Filter System Overview
            var systemOverview = await _submissionRepo.GetAllWithSelectorAsync(s => new DashboardEventDto
            {
                Id = s.Id,
                Poster = s.Poster,
                Name = s.EventNameEN,
                Category = new DropdownViewModel
                {
                    Id = s.Category
                          .Id,
                    Name = s.Category
                            .Name
                },
                Venue = new DropdownViewModel
                {
                    Id = s.Venue
                          .Id,
                    Name = s.Venue
                            .VenueName
                },
                VenueImage = s.Venue
                              .User
                              .ProfilePicture,
                Organizer = new IdentityDropdownDto
                {
                    Id = s.User
                          .Id,
                    Name = s.User
                            .FirstName +
                            " " +
                            s.User
                             .LastName
                },
                OrganizerImage = s.User
                                  .ProfilePicture,
                Date = s.Dates
                        .Select(ds => ds.Date
                                        .ToString("dd MMM, hh:mm tt"))
                        .ToList()
            },
            true,
            f => f.CreatedAt.Date <= _dateService.NowUtc.Date &&
                 (filter.SystemOverview == null ||
                  filter.SystemOverview == DashboardOption.Week) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddDays(-7) :
                                                                   (filter.SystemOverview == DashboardOption.Month) ? f.CreatedAt.Date >= _dateService.NowUtc.Date.AddMonths(-1) :
                                                                                                                      f.CreatedAt.Date >= _dateService.NowUtc.Date.AddYears(-1),
            o => o.OrderByDescending(x => x.Id));
            #endregion

            return new Response<DashboardDto>(new DashboardDto
            {
                Event = eventCount,
                EventOragnizer = organizerCount,
                Venue = venueCount,
                Users = userCount,
                TopCategories = categories,
                TopUsers = topUsers,
                TopVenues = venues,
                TopOrganizers = organizer,
                SystemOverview = systemOverview
            });
        }

        public async Task<Response<List<DropdownViewModel>>> Dropdown()
        {
            var dashboard = await Task.Run(() =>
            {
                var dropdownList = new List<DropdownViewModel>
                {
                    new DropdownViewModel
                    {
                        Id = (long) DashboardOption.Week,
                        Name = EnumExtention.GetDescriptionFromEnum(DashboardOption.Week)
                    },
                    new DropdownViewModel
                    {
                        Id = (long) DashboardOption.Month,
                        Name = EnumExtention.GetDescriptionFromEnum(DashboardOption.Month)
                    },
                    new DropdownViewModel
                    {
                        Id = (long) DashboardOption.Year,
                        Name = EnumExtention.GetDescriptionFromEnum(DashboardOption.Year)
                    }
                };

                return dropdownList;
            });

            return new Response<List<DropdownViewModel>>(dashboard);
        }
    }
}
