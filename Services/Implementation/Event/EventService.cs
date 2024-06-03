using AutoMapper;
using Core;
using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Enums;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.Event.Services;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.Shared.Services;
using DTOs.Shared;
using DTOs.Shared.Responses;

namespace Services.Implementation.Event
{
    internal sealed class EventService : IEventService
    {
        private readonly ISubmissionDateRepository _submissionDateRepo;
        private readonly ISubmissionRepository _submissionRepo;
        private readonly IOrganizerRepository _organizerRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ISubmissionCommentRepository _commentRepo;
        private readonly IFavouriteSubmissionRepository _favouriteSubmissionRepo;
        private readonly IBlockedCommentRepository _blockedCommentRepo;
        private readonly IAuthenticatedUserService _authService;
        private readonly IDateTimeService _dateService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EventService(ISubmissionDateRepository submissionDateRepo,
                            ISubmissionRepository submissionRepo,
                            IOrganizerRepository organizerRepo,
                            ICategoryRepository categoryRepo,
                            ISubmissionCommentRepository commentRepo,
                            IFavouriteSubmissionRepository favouriteSubmissionRepo,
                            IBlockedCommentRepository blockedCommentRepo,
                            IAuthenticatedUserService authService,
                            IDateTimeService dateService,
                            IUnitOfWork unitOfWork,
                            IMapper mapper)
        {
            _submissionDateRepo = submissionDateRepo;
            _submissionRepo = submissionRepo;
            _organizerRepo = organizerRepo;
            _categoryRepo = categoryRepo;
            _commentRepo = commentRepo;
            _favouriteSubmissionRepo = favouriteSubmissionRepo;
            _blockedCommentRepo = blockedCommentRepo;
            _authService = authService;
            _dateService = dateService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IList<ListEventMobileDto>>> GetMobilePagination(EventMobileListParameters filter)
        {
            var result = await _submissionDateRepo.GetEventsMobile(filter,
                                                                   f => f.Submission
                                                                         .IsApproved &&
                                                                        f.Date
                                                                         .Date >= _dateService.NowUtc
                                                                                              .Date,
                                                                   _authService.UserId!);
            if (result.Count == 0 ||
                result.Data == null)
            {
                return new PagedResponse<IList<ListEventMobileDto>>(null, filter.PageNumber, filter.PageSize);
            }
            return new PagedResponse<IList<ListEventMobileDto>>(result.Data, filter.PageNumber, filter.PageSize, result.Count);

        }

        public async Task<PagedResponse<IList<ListEventDto>>> GetPagination(EventListParameters filter)
        {
            var result = await _submissionDateRepo.GetEvents(filter,
                                                              f => f.Date
                                                                    .Date >= _dateService.NowUtc
                                                                                         .Date);
            if (result.Count == 0 ||
                result.Data == null)
            {
                return new PagedResponse<IList<ListEventDto>>(null, filter.PageNumber, filter.PageSize);
            }
            return new PagedResponse<IList<ListEventDto>>(result.Data, filter.PageNumber, filter.PageSize, result.Count);
        }

        public async Task<Response<bool>> ToggleStatus(ToggleSubmissionStatusDto request)
        {
            var submission = await _submissionDateRepo.GetByIdAsync(request.Id,
                                                                    f => !f.IsCancelled &&
                                                                         f.Date
                                                                          .Date >= _dateService.NowUtc
                                                                                               .Date,
                                                                    i => i.Submission);
            if (submission is null)
            {
                return new Response<bool>("Submission is not found or expired.");
            }

            submission.Submission.Status = request.Status;

            if (request.Status == SubmissionStatus.ACCEPT)
            {
                submission.Submission.IsApproved = true;
            }

            _submissionRepo.Update(submission.Submission);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> ToggleSpotlight(ToggleSubmissionSpotlightDto request)
        {
            var submission = await _submissionDateRepo.GetByIdAsync(request.Id,
                                                                    f => !f.IsCancelled &&
                                                                         f.Date
                                                                          .Date >= _dateService.NowUtc
                                                                                               .Date,
                                                                    i => i.Submission);
            if (submission is null)
            {
                return new Response<bool>("Submission is not found or expired.");
            }

            if (submission.Submission.IsSpotlight)
            {
                submission.Submission.IsSpotlight = false;
            }
            else
            {
                submission.Submission.IsSpotlight = true;
            }

            _submissionRepo.Update(submission.Submission);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<List<DropdownViewModel>>> Dropdown()
        {
            var status = await Task.Run(() =>
            {
                var dropdownList = new List<DropdownViewModel>
                {
                    new DropdownViewModel
                    {
                        Id = (long) SubmissionStatus.ACCEPT,
                        Name = SubmissionStatus.ACCEPT.ToString()
                    },
                    new DropdownViewModel
                    {
                        Id = (long) SubmissionStatus.REJECT,
                        Name = SubmissionStatus.REJECT.ToString()
                    }
                };

                return dropdownList;
            });

            return new Response<List<DropdownViewModel>>(status);
        }

        public async Task<Response<EventDetailDto>> Detail(long id)
        {
            var result = await _submissionDateRepo.GetPropertyWithSelectorAsync(s => new EventDetailDto
            {
                Id = s.Id,
                Poster = s.Submission.Poster,
                Category = new CategoryDropdownDto
                {
                    Id = s.Submission.Category.Id,
                    Name = s.Submission.Category.Name,
                    Image = s.Submission.Category.Image
                },
                IsFavourite = s.Submission
                               .FavouriteSubmissions
                               .Any(c => c.SubmissionId == s.SubmissionId &&
                                         c.UserId == _authService.UserId!),
                Name = s.Submission.EventNameEN,
                Organizer = new IdentityDropdownDto
                {
                    Id = s.Submission.User.Id,
                    Name = s.Submission.User.FirstName + " " + s.Submission.User.LastName
                },
                Description = s.Submission.EventDescriptionEN,
                Date = s.Date.ToString("dd-MM-yyyy, hh:mm tt"),
                AttendanceOption = new DropdownViewModel
                {
                    Id = (long)s.Submission.AttendanceOption,
                    Name = s.Submission.AttendanceOption.ToString()
                },
                URL = s.Submission.URL,
                PaymentFee = s.Submission.PaymentFee,
                Address = s.Submission.Branch.Address,
                MapLink = s.Submission.Branch.MapLink,
                Venue = new UserDetailDto
                {
                    Id = s.Submission.Venue.Id,
                    Name = s.Submission.Venue.User.FirstName + " " + s.Submission.Venue.User.LastName,
                    Photo = s.Submission.Venue.User.ProfilePicture,
                    Address = s.Submission.Branch.Address,
                    MapLink = s.Submission.Branch.MapLink,
                    Instagram = s.Submission.Venue.Instagram,
                    Facebook = s.Submission.Venue.Facebook,
                    WebsiteURL = s.Submission.Venue.WebsiteURL,
                    Other = s.Submission.Venue.Other
                },
                EventOrganizer = new UserDetailDto()
            }, true,
            f => !f.IsCancelled && f.Id == id);

            if (result is null)
            {
                return new Response<EventDetailDto>("Event not found.");
            }

            var organizer = await _organizerRepo.GetPropertyWithSelectorAsync(s => new UserDetailDto
            {
                Id = s.Id,
                Name = s.User.FirstName + " " + s.User.LastName,
                Address = s.Address,
                Facebook = s.Facebook,
                Instagram = s.Instagram,
                MapLink = s.MapLink,
                Other = s.Other,
                Photo = s.User.ProfilePicture,
                WebsiteURL = s.WebsiteURL
            }, true,
            f => f.UserId == result.Organizer.Id);

            result.EventOrganizer = organizer!;

            return new Response<EventDetailDto>(result);
        }

        public async Task<Response<HomeDto>> Home()
        {
            var categories = await _categoryRepo.GetAllWithSelectorAsync(s => new CategoryDropdownDto
            {
                Id = s.Id,
                Image = s.Image,
                Name = s.Name
            },
            true,
            null,
            o => o.OrderByDescending(x => x.Id));

            #region Filter
            var spotlightFilter = new HomeEventListParameters
            {
                IsSpotlight = true,
                OrderSpotlight = true,
                OrganizerId = null,
                VenueId = null,
            };

            var todayFilter = new HomeEventListParameters
            {
                IsSpotlight = null,
                OrderSpotlight = false,
                OrganizerId = null,
                VenueId = null
            };
            #endregion

            var spotlight = await _submissionDateRepo.Home(spotlightFilter,
                                                           f => f.Date
                                                                 .Date >= _dateService.NowUtc
                                                                                      .Date);

            var today = await _submissionDateRepo.Home(todayFilter,
                                                       f => f.Date
                                                             .Date == _dateService.NowUtc
                                                                                  .Date);

            return new Response<HomeDto>(new HomeDto
            {
                Category = categories,
                Spotlight = spotlight,
                Today = today
            });
        }

        public async Task<Response<bool>> ToggleFavourite(ToggleSubmissionFavouriteDto request)
        {
            var submissionId = await _submissionDateRepo.GetPropertyWithSelectorAsync(s => s.SubmissionId,
                                                                                      true,
                                                                                      f => !f.IsCancelled &&
                                                                                           f.Id == request.SubmissionId);
            if (submissionId == 0)
            {
                return new Response<bool>($"Wrong submission id: {request.SubmissionId}.");
            }

            var UserSubmission = await _favouriteSubmissionRepo.Get(_authService.UserId!, submissionId);

            if (UserSubmission is null)
            {
                _favouriteSubmissionRepo.Add(new FavouriteSubmission
                {
                    UserId = _authService.UserId!,
                    SubmissionId = submissionId,
                });
            }
            else
            {
                _favouriteSubmissionRepo.Remove(UserSubmission);
            }

            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> Comment(CommentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Comment))
            {
                return new Response<bool>($"Comment cannot be empty. Please provide your feedback or observations.");
            }
            var checkBlocked = await _blockedCommentRepo.Exists(f => f.UserId == _authService.UserId!);
            if (checkBlocked)
            {
                return new Response<bool>($"you are blocked from comment.");
            }

            var submissionId = await _submissionDateRepo.GetPropertyWithSelectorAsync(s => s.SubmissionId,
                                                                                      true,
                                                                                      f => !f.IsCancelled &&
                                                                                           f.Id == request.Id);
            if (submissionId == 0)
            {
                return new Response<bool>($"Wrong submission id: {request.Id}.");
            }

            var submissionComment = new SubmissionCommentDto
            {
                Comment = request.Comment,
                SubmissionId = submissionId,
                UserId = _authService?.UserId!
            };

            var result = _commentRepo.Add(_mapper.Map<SubmissionComment>(submissionComment));
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<FeeDto>> FeeConfiguration()
        {
            var min = await _submissionRepo.MinFee();
            var max = await _submissionRepo.MaxFee();

            return new Response<FeeDto>(new FeeDto
            {
                Max = max,
                Min = min
            });
        }

        public async Task<Response<bool>> SpotlightOrder(SpotlightOrderDto request)
        {
            var entity = await _submissionDateRepo.Exists(f => f.Id == request.Id);
            if (!entity)
            {
                return new Response<bool>("Submission Id not found.");
            }

            await _submissionDateRepo.SpotlightOrder(request.Id, request.SpotlightOrder);
            return new Response<bool>(true);
        }
    }
}
