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
    internal sealed class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepo;
        private readonly ISubmissionDateRepository _submissionDateRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IVenueRepository _venueRepo;
        private readonly IOrganizerRepository _organizerRepo;
        private readonly IBranchRepository _branchRepo;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubmissionService(ISubmissionRepository submissionRepo,
                                 ISubmissionDateRepository submissionDateRepo,
                                 ICategoryRepository categoryRepo,
                                 IVenueRepository venueRepo,
                                 IOrganizerRepository organizerRepo,
                                 IBranchRepository branchRepo,
                                 IAuthenticatedUserService authenticatedUserService,
                                 IUnitOfWork unitOfWork,
                                 IMapper mapper)
        {
            _submissionRepo = submissionRepo;
            _submissionDateRepo = submissionDateRepo;
            _categoryRepo = categoryRepo;
            _venueRepo = venueRepo;
            _organizerRepo = organizerRepo;
            _branchRepo = branchRepo;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<long>> Add(AddSubmissionDto request)
        {
            #region Validation
            if (!await _categoryRepo.Exists(f => f.Id == request.CategoryId))
            {
                return new Response<long>($"Category Id: {request.CategoryId} not found.");
            }
            if (!await _venueRepo.Exists(f => f.Id == request.VenueId))
            {
                return new Response<long>($"Venue Id: {request.VenueId} not found.");
            }
            if (!await _branchRepo.Exists(f => f.Id == request.BranchId &&
                                               f.VenueId == request.VenueId))
            {
                return new Response<long>($"Branch Id: {request.BranchId} not found.");
            }
            if (request.Dates.Count == 0)
            {
                return new Response<long>($"Event Dates is required.");
            }
            #endregion

            request.ExpandDates();

            var submission = _mapper.Map<Submission>(request);
            submission.UserId = _authenticatedUserService.UserId!;
            var result = _submissionRepo.Add(submission);
            await _unitOfWork.SaveAsync();

            return new Response<long>(result.Id);
        }

        public async Task<Response<SubmissionDropdownDto>> Dropdown()
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


            IList<DropdownViewModel> dropdownViewModels = new List<DropdownViewModel>();

            var organizers = await _organizerRepo.GetAllWithSelectorAsync(s => new DropdownViewModel
            {
                Id = s.Id,
                Name = s.User.FirstName + " " + s.User.LastName,

            },
            true,
            f => !f.IsDeleted && !f.User.IsBlocked,
            o => o.OrderByDescending(x => x.Id));

            var venues = await _venueRepo.GetAllWithSelectorAsync(s => new VenueDropdownDto
            {
                Id = s.Id,
                Name = s.VenueName,
                Branch = s.Branches
                          .Where(f => !f.IsDeleted &&
                                      f.VenueId == s.Id && !s.User.IsBlocked)
                          .Select(bs => new DropdownViewModel
                          {
                              Id = bs.Id,
                              Name = bs.Name
                          })
                          .ToList()
            },
            true,
            f => !f.IsDeleted && !f.User.IsBlocked,
            o => o.OrderByDescending(x => x.Id));

            dropdownViewModels = organizers;

            foreach (var i in venues)
            {
                DropdownViewModel dropdownViewModel = new DropdownViewModel();
                dropdownViewModel.Id = i.Id;
                dropdownViewModel.Name = i.Name;
                dropdownViewModels.Add(dropdownViewModel);
            }

            return new Response<SubmissionDropdownDto>(new SubmissionDropdownDto
            {
                Category = categories,
                Venue = venues,
                Organizer = dropdownViewModels,
                Repeat = RepeatDropdown(),
                Attendance = AttendanceDropdown()
            });
        }

        public async Task<Response<long>> AdminAdd(AddAdminSubmissionDto request)
        {
            #region Validation
            if (!await _categoryRepo.Exists(f => f.Id == request.CategoryId))
            {
                return new Response<long>($"Category Id: {request.CategoryId} not found.");
            }
            if (!await _venueRepo.Exists(f => f.Id == request.VenueId))
            {
                return new Response<long>($"Venue Id: {request.VenueId} not found.");
            }
            /*if (!await _organizerRepo.Exists(f => f.Id == request.OrganizerId))
            {
                return new Response<long>($"Oragnization Id: {request.OrganizerId} not found.");
            }*/
            if (!await _branchRepo.Exists(f => f.Id == request.BranchId &&
                                               f.VenueId == request.VenueId))
            {
                return new Response<long>($"Branch Id: {request.BranchId} not found.");
            }
            var userId = await _organizerRepo.GetPropertyWithSelectorAsync(s => s.UserId,
                                                                           true,
                                                                           f => f.Id == request.OrganizerId);
            var userIdVenu = await _venueRepo.GetPropertyWithSelectorAsync(s => s.UserId,
                                                                           true,
                                                                           f => f.Id == request.VenueId);
            #endregion


            request.ExpandDates();
            var submission = _mapper.Map<Submission>(request);
            var result = _submissionRepo.Add(submission);
            if (userId is null)
            {
                //return new Response<long>($"Organizer Id: {request.OrganizerId} not found.");
                //return new Response<long>($"User Id not found.");
                submission.UserId = userIdVenu;
            }
            else
            {
                submission.UserId = userId;
            }
            await _unitOfWork.SaveAsync();

            return new Response<long>(result.Id);
        }

        public async Task<Response<bool>> Update(UpdateAdminSubmissionDto request)
        {
            #region Validation
            var entity = await _submissionRepo.GetByIdAsync(request.Id);
            if (entity is null)
            {
                return new Response<bool>($"Submission Id: {request.Id} not found.");
            }
            if (!await _categoryRepo.Exists(f => f.Id == request.CategoryId))
            {
                return new Response<bool>($"Category Id: {request.CategoryId} not found.");
            }
            if (!await _venueRepo.Exists(f => f.Id == request.VenueId))
            {
                return new Response<bool>($"Venue Id: {request.VenueId} not found.");
            }
            /*if (!await _organizerRepo.Exists(f => f.Id == request.OrganizerId))
            {
                return new Response<bool>($"Organization Id: {request.VenueId} not found.");
            }*/
            if (!await _branchRepo.Exists(f => f.Id == request.BranchId &&
                                               f.VenueId == request.VenueId))
            {
                return new Response<bool>($"Branch Id: {request.BranchId} not found.");
            }
            /*var userId = await _organizerRepo.GetPropertyWithSelectorAsync(s => s.UserId,
                                                                           true,
                                                                           f => f.Id == request.OrganizerId);
            if (userId is null)
            {
                return new Response<bool>($"Organizer Id: {request.OrganizerId} not found.");
            }*/
            #endregion

            await _submissionDateRepo.DeleteSubmissionDates(request.Id);

            _mapper.Map(request, entity);

            _submissionRepo.Update(entity);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        private List<RepeatDropdownDto> RepeatDropdown()
        {
            return new List<RepeatDropdownDto>
            {
                new RepeatDropdownDto
                {
                    Id = 1,
                    Number = 1
                },
                new RepeatDropdownDto
                {
                    Id = 2,
                    Number = 2
                },
                new RepeatDropdownDto
                {
                    Id = 3,
                    Number = 3
                }
            };
        }

        private List<DropdownViewModel> AttendanceDropdown()
        {
            return new List<DropdownViewModel>
            {
                new DropdownViewModel
                {
                    Id = (long) AttendanceOption.Registration,
                    Name = AttendanceOption.Registration.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long) AttendanceOption.Ticket,
                    Name = AttendanceOption.Ticket.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long) AttendanceOption.Free,
                    Name = AttendanceOption.Free.ToString()
                }
            };
        }

        public async Task<Response<AddAdminSubmissionDto>> GetDetails(long request)
        {
            var data = await _submissionRepo.GetByIdAsync(request);
            var listdate = await _submissionDateRepo.GetBySubmissionId(request);
            var submission = _mapper.Map<AddAdminSubmissionDto>(data);
            submission.Dates.Add(listdate[0].Date);
            submission.Dates.Add(listdate[listdate.Count() - 1].Date);
            return new Response<AddAdminSubmissionDto>(submission);
        }

        public async Task<Response<SubmissionDetailDto>> Detail(long id)
        {
            var result = await _submissionRepo.GetPropertyWithSelectorAsync(s => new SubmissionDetailDto
            {
                Id = s.Id,
                EventNameEN = s.EventNameEN,
                EventNameAR = s.EventNameAR,
                EventDescriptionEN = s.EventDescriptionEN,
                EventDescriptionAR = s.EventDescriptionAR,
                MainArtestNameEN = s.MainArtestNameEN,
                MainArtestNameAR = s.MainArtestNameAR,
                KidsAvailability = s.KidsAvailability,
                AttendanceOption = new DropdownViewModel
                {
                    Id = (long)s.AttendanceOption,
                    Name = s.AttendanceOption.ToString()
                },
                URL = s.URL,
                PaymentFee = s.PaymentFee,
                Poster = s.Poster,
                ContactPerson = s.ContactPerson,
                AddtionalComment = s.AddtionalComment,
                IsApproved = s.IsApproved,
                IsSpotlight = s.IsSpotlight,
                Status = new DropdownViewModel
                {
                    Id = (long)s.Status,
                    Name = s.Status.ToString()
                },
                Category = new CategoryDropdownDto
                {
                    Id = s.Category.Id,
                    Name = s.Category.Name,
                    Image = s.Category.Image
                },
                Venue = new UserDetailDto
                {
                    Id = s.Venue.Id,
                    Name = s.Venue.User.FirstName + " " + s.Venue.User.LastName,
                    Photo = s.Venue.User.ProfilePicture,
                    Address = s.Branch.Address,
                    MapLink = s.Branch.MapLink,
                    Instagram = s.Venue.Instagram,
                    Facebook = s.Venue.Facebook,
                    WebsiteURL = s.Venue.WebsiteURL,
                    Other = s.Venue.Other
                },
                Branch = new DropdownViewModel
                {
                    Id = s.Branch.Id,
                    Name = s.Branch.Name
                },
                Date = s.Dates
                        .Select(ds => ds.Date
                                        .ToString("dd-MM-yyyy, hh:mm tt"))
                        .ToList(),
                Organizer = new UserDetailDto(),
                EventOrganizer = new IdentityDropdownDto
                {
                    Id = s.User.Id,
                    Name = s.User.FirstName + " " + s.User.LastName
                }
            }, true,
            f => f.Id == id);

            if (result is null)
            {
                return new Response<SubmissionDetailDto>("Event not found.");
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
            f => f.UserId == result.EventOrganizer.Id);

            result.Organizer = organizer!;

            return new Response<SubmissionDetailDto>(result);
        }
    }
}
