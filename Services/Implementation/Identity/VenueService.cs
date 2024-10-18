using AutoMapper;
using Core;
using Core.DTOs.Event.Response;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Entities.Identity;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.Shared.Services;
using DTOs.Shared;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text.RegularExpressions;

namespace Services.Implementation.Identity
{
    internal sealed class VenueService : IVenueService
    {
        private readonly IOrganizerRepository _organizerRepo;
        private readonly IVenueRepository _venueRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationHelperService _authHelperService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IFacilityRepository _facilityRepo;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public VenueService(IOrganizerRepository organizerRepo, 
                            IVenueRepository venueRepo,
                            IUnitOfWork unitOfWork,
                            RoleManager<Role> roleManager,
                            UserManager<User> userManager,
                            IAuthenticationHelperService authHelperService,
                            IDateTimeService dateTimeService,
                            ICategoryRepository categoryRepo,
                            IFacilityRepository facilityRepo,
                            IMapper mapper,
                            IAuthenticatedUserService authenticatedUserService)
        {
            _organizerRepo = organizerRepo;
            _venueRepo = venueRepo;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _authHelperService = authHelperService;
            _dateTimeService = dateTimeService;
            _categoryRepo = categoryRepo;
            _facilityRepo = facilityRepo;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<PagedResponse<IList<ListVenueDto>>> GetPagination(VenueParameters filter)
        {
            var result = await _venueRepo.GetPaginaton(filter);
            if (result.Count == 0 ||
                result.Data == null)
            {
                return new PagedResponse<IList<ListVenueDto>>(null, filter.PageNumber, filter.PageSize);
            }
            return new PagedResponse<IList<ListVenueDto>>(result.Data, filter.PageNumber, filter.PageSize, result.Count);
        }

        public async Task<Response<string>> Register(RegisterVenueDto request)
        {
            #region Validation
            if (!Regex.IsMatch(request.User.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!”#$%&’()*+,-./:;<=>?@[\]^_`{|}~']).{8,}$"))
            {
                return new Response<string>($"Password format should contain At Least Upper Case letter, lower Case letter, Special Character, and Number.");
            }

            var role = await _roleManager.FindByIdAsync("e35a5541-be51-44a2-959a-f957d1142e3b");
            if (role is null)
            {
                return new Response<string>($"Role not exist.");
            }

            if (request.Venue is not null &&
                !await _categoryRepo.Exists(f => f.Id == request.Venue.CategoryId))
            {
                return new Response<string>($"Category: {request.Venue.CategoryId} not found.");
            }
            #endregion

            var user = _mapper.Map<User>(request.User);

            #region Filling data
            user.CreatedBy = _authenticatedUserService.UserId!;
            user.CreatedAt = _dateTimeService.NowUtc;
            string[] emailSplit = user.Email!.Split("@");
            user.UserName = emailSplit[0];
            user.NormalizedUserName = emailSplit[0].ToUpper();
            _authHelperService.CreatePasswordHash(request.User.Password, out string passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            #endregion

            var resultUser = await _userManager.CreateAsync(user);
            if (resultUser.Succeeded)
            {
                var resultUserRole = await _userManager.AddToRoleAsync(user, role.Name!);

                if (resultUserRole.Succeeded)
                {
                    var venue = _mapper.Map<Venue>(request.Venue);
                    venue.UserId = user.Id;

                    int orgId = await _organizerRepo.GetLastTaskOrderId();
                    int venuId = await _venueRepo.GetLastTaskOrderId();
                    int max = GetMax(orgId, venuId);
                    venue.Id = max + 1;
                    _venueRepo.Add(venue);
                    await _unitOfWork.SaveAsync();
                    /*var entity = await _organizerRepo.GetByIdAsync(venue.Id);
                    if(entity != null) {
                        int valueId = await _organizerRepo.GetLastTaskOrderId();
                        venue.Id = valueId + 1;
                        _venueRepo.Update(venue);
                        await _unitOfWork.SaveAsync();
                    }*/

                    return new Response<string>(user.Id, "Client created successfully");
                }
                return new Response<string>("Can't created Client right now.", resultUserRole.Errors.Select(s => s.Description).ToList());
            }
            else
            {
                if (resultUser.Errors is not null)
                {
                    return new Response<string>("Can't created Client right now.", resultUser.Errors.Select(s => s.Description).ToList());
                }

                return new Response<string>("Can't created Client right now.");
            }
        }
        public static int GetMax(int first, int second)
        {
            return first > second ? first : second;
        }
        public async Task<Response<RegisterVenueDropdownDto>> Dropdown()
        {
            var categories = await _categoryRepo.GetAllWithSelectorAsync(s => new DropdownViewModel
            {
                Id = s.Id,
                Name = s.Name
            },
            true,
            null,
            o => o.OrderByDescending(x => x.Id));

            var facilities = await _facilityRepo.GetAllWithSelectorAsync(s => new ListFacilityDto
            {
                Id = s.Id,
                ImageName = s.ImageName,
                ImagePath = s.ImagePath
            },
            true,
            null,
            o => o.OrderByDescending(x => x.Id));

            var days = DayDropdown();

            return new Response<RegisterVenueDropdownDto>(new RegisterVenueDropdownDto
            {
                Category = categories,
                Facility = facilities,
                Days = days
            });
        }

        private List<DropdownViewModel> DayDropdown()
        {
            return new List<DropdownViewModel>
            {
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Saturday,
                    Name = DayOfWeek.Saturday.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Sunday,
                    Name = DayOfWeek.Sunday.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Monday,
                    Name = DayOfWeek.Monday.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Tuesday,
                    Name = DayOfWeek.Tuesday.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Wednesday,
                    Name = DayOfWeek.Wednesday.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Thursday,
                    Name = DayOfWeek.Thursday.ToString()
                },
                new DropdownViewModel
                {
                    Id = (long)DayOfWeek.Friday,
                    Name = DayOfWeek.Friday.ToString()
                }
            };
        }

        public async Task<Response<VenueDetailDto>> Detail(long id)
        {
            var result = await _venueRepo.GetPropertyWithSelectorAsync(s => new VenueDetailDto
            {
                User = new UserDetailViewModel
                {
                    Id = s.User.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Email = s.User.Email!,
                    ProfilePicture = s.User.ProfilePicture
                },
                Id = s.Id,
                Category = new CategoryDetailDto
                {
                    Id = s.Category.Id,
                    Image = s.Category.Image,
                    Name = s.Category.Name
                },
                Facebook = s.Facebook,
                Instagram = s.Instagram,
                Other = s.Other,
                PhoneNumber = s.PhoneNumber,
                VenueDescription = s.VenueDescription,
                VenueName = s.VenueName,
                WebsiteURL = s.WebsiteURL,
                Photos = s.Photos
                          .Where(f => !f.IsDeleted &&
                                      f.VenueId == id)
                          .Select(ps => ps.Image)
                          .ToList(),
                Facilities = s.VenueFacilities
                              .Where(f => !f.IsDeleted &&
                                          f.VenueId == id)
                              .Select(vs => new FacilityDetailDto
                              {
                                  Id = vs.Id,
                                  ImageName = vs.Facility.ImageName,
                                  ImagePath = vs.Facility.ImagePath,
                              })
                              .ToList(),
                Branches = s.Branches
                            .Where(f => !f.IsDeleted &&
                                        f.VenueId == id)
                            .Select(bs => new BranchDetailDto
                            {
                                Id = bs.Id,
                                Address = bs.Address,
                                MapLink = bs.MapLink,
                                Name = bs.Name,
                                WorkDay = bs.WorkDays
                                            .Where(f => !f.IsDeleted &&
                                                        f.BranchId == bs.Id)
                                            .Select(wds => new WorkDayDetailDto
                                            {
                                                Id = wds.Id,
                                                From = wds.From,
                                                To = wds.To,
                                                Day = new DropdownViewModel
                                                {
                                                    Id = (long)wds.Day,
                                                    Name = wds.Day.ToString()
                                                }
                                            })
                                            .ToList()
                            })
                            .ToList()
            }, true,
            f => f.Id == id && !f.IsDeleted);

            if (result is null)
            {
                return new Response<VenueDetailDto>(null, "No Data Found.");
            }

            return new Response<VenueDetailDto>(result!);
        }
    }
}
