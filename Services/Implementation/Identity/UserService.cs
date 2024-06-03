using AutoMapper;
using Core.DTOs.Event.Response;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.Role;
using Core.DTOs.Shared;
using Core.DTOs.User;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Identity;
using Core.Enums;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.Shared.Services;
using Dapper;
using DTOs.Shared;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Services.Implementation.Identity
{
    internal sealed class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthenticationHelperService _authHelperService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly IAuthenticatedUserService _authService;
        private readonly IVenueRepository _venueRepo;
        private readonly IOrganizerRepository _organizerRepo;
        public UserService(UserManager<User> userManager,
                           RoleManager<Role> roleManager,
                           IAuthenticationHelperService authHelperService,
                           ICategoryRepository categoryRepo,
                           IMapper mapper,
                           IDateTimeService dateTimeService,
                           IAuthenticatedUserService authService,
                           IVenueRepository venueRepo,
                           IOrganizerRepository organizerRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authHelperService = authHelperService;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _authService = authService;
            _venueRepo = venueRepo;
            _organizerRepo = organizerRepo;
        }

        public async Task<PagedResponse<List<UserDto>>> GetPagination(PaginationParameter filter)
        {
            var users = await _userManager.Users
                                          .OrderByDescending(f => f.CreatedAt)
                                          .Skip((filter.PageNumber - 1) * filter.PageSize)
                                          .Take(filter.PageSize)
                                          .Select(s => new UserDto
                                          {
                                              Id = s.Id,
                                              Email = s.Email!,
                                              FirstName = s.FirstName,
                                              LastName = s.LastName,
                                              Photo = s.ProfilePicture,
                                              Status = new DropdownViewModel
                                              {
                                                  Id = (long)s.Status,
                                                  Name = s.Status.ToString()
                                              },
                                              IsBlocked = s.IsBlocked
                                          })
                                          .AsNoTracking()
                                          .ToListAsync();

            var count = await _userManager.Users
                                          .CountAsync();

            return new PagedResponse<List<UserDto>>(users, filter.PageNumber, filter.PageSize, count);
        }

        public async Task<PagedResponse<IReadOnlyList<ListAdminDto>>> GetAdminPagination(ListAdminParameters filter)
        {
            if (filter.RoleFilter != null &&
                filter.RoleFilter != "Admin" &&
                filter.RoleFilter != "Super Admin")
            {
                return new PagedResponse<IReadOnlyList<ListAdminDto>>("Invalid role filter. Role filter must be 'Admin', 'Super Admin', or null.");
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Search", filter.Search);
            parameters.Add("@RoleFilter", filter.RoleFilter == null ? "Both" : filter.RoleFilter);
            parameters.Add("@PageNumber", filter.PageNumber);
            parameters.Add("@PageSize", filter.PageSize);

            var admins = await _categoryRepo.QueryMultipleAsync<int, ListAdminDto>("[dbo].[SP_GetAdminWithPagination]", parameters, System.Data.CommandType.StoredProcedure);

            if (admins is not null && admins.Item1[0] > 0)
            {
                return new PagedResponse<IReadOnlyList<ListAdminDto>>(admins.Item2.AsList(), filter.PageNumber, filter.PageSize, admins.Item1[0]);
            }

            return new PagedResponse<IReadOnlyList<ListAdminDto>>(null, filter.PageNumber, filter.PageSize);
        }

        public async Task<PagedResponse<IReadOnlyList<ListAdminDto>>> GetAudiencePagination(ListAdminParameters filter)
        {            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Search", filter.Search);
            parameters.Add("@RoleFilter", filter.RoleFilter == "Audience");
            parameters.Add("@PageNumber", filter.PageNumber);
            parameters.Add("@PageSize", filter.PageSize);

            var admins = await _categoryRepo.QueryMultipleAsync<int, ListAdminDto>("[dbo].[SP_GetAudienceWithPagination]", parameters, System.Data.CommandType.StoredProcedure);

            if (admins is not null && admins.Item1[0] > 0)
            {
                return new PagedResponse<IReadOnlyList<ListAdminDto>>(admins.Item2.AsList(), filter.PageNumber, filter.PageSize, admins.Item1[0]);
            }

            return new PagedResponse<IReadOnlyList<ListAdminDto>>(null, filter.PageNumber, filter.PageSize);
        }

        public async Task<Response<bool>> ToggleStatus(ToggleStatusDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                return new Response<bool>("User is not Found.");
            }

            user.Status = request.Status;

            await _userManager.UpdateAsync(user);

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> ToggleBlock(ToggleBlockDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                return new Response<bool>("User is not Found.");
            }

            if (!user.IsBlocked)
            {
                user.IsBlocked = true;
            }
            else
            {
                user.IsBlocked = false;
            }
            await _userManager.UpdateAsync(user);

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
                        Id = (long) Status.Normal,
                        Name = Status.Normal.ToString()
                    },
                    new DropdownViewModel
                    {
                        Id = (long) Status.Premium,
                        Name = Status.Premium.ToString()
                    }
                };

                return dropdownList;
            });

            return new Response<List<DropdownViewModel>>(status);
        }

        public async Task<Response<UserDto>> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return new Response<UserDto>("User not found.");
            }

            return new Response<UserDto>(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Photo = user.ProfilePicture,
                Status = new DropdownViewModel
                {
                    Id = (long)user.Status,
                    Name = user.Status.ToString()
                },
                IsBlocked = user.IsBlocked
            });
        }

        public async Task<Response<MobileUserDetailDto>> Profile()
        {
            var user = await _userManager.FindByIdAsync(_authService.UserId!);
            if (user is null)
            {
                return new Response<MobileUserDetailDto>("User not found.");
            }

            var venue = await _venueRepo.GetPropertyWithSelectorAsync(s => new VenueProfileDto
            {
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
                PhoneNumber = $"+2{s.PhoneNumber}",
                VenueDescription = s.VenueDescription,
                VenueName = s.VenueName,
                VenueImage = s.User.ProfilePicture,
                WebsiteURL = s.WebsiteURL,
                Schedule = s.Submissions
                            .Where(f => f.VenueId == s.Id &&
                                        f.Dates.Any(df => !df.IsDeleted &&
                                                          df.Date
                                                            .Date >= _dateTimeService.NowUtc
                                                                                     .Date))
                            .Select(ss => new ScheduleEventDto
                            {
                                Id = ss.Id,
                                CategoryPoster = ss.Category.Image,
                                Name = ss.EventNameEN,
                                Poster = ss.Poster,
                                Date = ss.Dates
                                         .Where(df => df.SubmissionId == ss.Id &&
                                                      !df.IsDeleted &&
                                                      df.Date
                                                        .Date >= _dateTimeService.NowUtc
                                                                                 .Date)
                                         .Select(ds => ds.Date.ToString("dd MMM, hh:mm tt"))
                                         .ToList()
                            }).ToList(),
                Photos = s.Photos
                          .Where(f => !f.IsDeleted &&
                                      f.VenueId == s.Id)
                          .Select(ps => ps.Image)
                          .ToList(),
                Facilities = s.VenueFacilities
                              .Where(f => !f.IsDeleted &&
                                          f.VenueId == s.Id)
                              .Select(vs => new FacilityDetailDto
                              {
                                  Id = vs.Id,
                                  ImageName = vs.Facility.ImageName,
                                  ImagePath = vs.Facility.ImagePath,
                              })
                              .ToList(),
                Branches = s.Branches
                            .Where(f => !f.IsDeleted &&
                                        f.VenueId == s.Id)
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
            f => f.UserId == _authService.UserId && !f.IsDeleted);

            var organizer = await _organizerRepo.GetPropertyWithSelectorAsync(s => new OrganizerProfileDto
            {
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
                WebsiteURL = s.WebsiteURL,
                Address = s.Address,
                Description = s.Description,
                MapLink = s.MapLink
            }, true,
            f => f.UserId == _authService.UserId && !f.IsDeleted);

            return new Response<MobileUserDetailDto>(new MobileUserDetailDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Photo = user.ProfilePicture,
                Phone = user.PhoneNumber,
                Organizer = organizer,
                Venue = venue
            });
        }

        public async Task<Response<bool>> Update(UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                return new Response<bool>("User is not Found.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.ProfilePicture = request.Photo;

            await _userManager.UpdateAsync(user);

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> Update(UserProfileDto request)
        {
            var user = await _userManager.FindByIdAsync(_authService.UserId!);
            if (user is null)
            {
                return new Response<bool>("User is not Found.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;
            user.ProfilePicture = request.Photo;

            await _userManager.UpdateAsync(user);

            return new Response<bool>(true);
        }

        public async Task<Response<List<RoleViewModel>>> AdminDropdown()
        {
            var roles = await _roleManager.Roles
                                          .Where(f => f.NormalizedName == "SUPER ADMIN" ||
                                                      f.NormalizedName == "ADMIN")
                                          .Select(c => new RoleViewModel
                                          {
                                              Id = c.Id,
                                              Name = c.Name!
                                          })
                                          .OrderBy(x => x.Name)
                                          .AsNoTracking()
                                          .ToListAsync();

            return new Response<List<RoleViewModel>>(roles);
        }

        public async Task<Response<string>> RegisterAdmin(UserAddModel request)
        {
            #region Validation
            if (!Regex.IsMatch(request.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!”#$%&’()*+,-./:;<=>?@[\]^_`{|}~']).{8,}$"))
            {
                return new Response<string>($"Password format should contain At Least Upper Case letter, lower Case letter, Special Character, and Number.");
            }

            if (request.RoleId != "12fb541f-24f6-450a-86b6-8747fb8c3ff0" &&
                request.RoleId != "cbbcbc2d-b9a2-43a3-8894-576e4d2351e1")
            {
                return new Response<string>($"Role not allowed.");
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role is null)
            {
                return new Response<string>($"Role not exist.");
            }
            #endregion

            var user = _mapper.Map<User>(request);

            #region Filling data
            user.CreatedBy = _authService.UserId!;
            user.CreatedAt = _dateTimeService.NowUtc;
            string[] emailSplit = user.Email!.Split("@");
            user.UserName = emailSplit[0];
            user.NormalizedUserName = emailSplit[0].ToUpper();
            _authHelperService.CreatePasswordHash(request.Password, out string passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            #endregion

            var resultUser = await _userManager.CreateAsync(user);
            if (resultUser.Succeeded)
            {
                var resultUserRole = await _userManager.AddToRoleAsync(user, role.Name!);
                if (resultUserRole.Succeeded)
                {
                    return new Response<string>(user.Id, "User created successfully");
                }
                return new Response<string>("Can't created User right now.", resultUserRole.Errors.Select(s => s.Description).ToList());
            }
            else
            {
                if (resultUser.Errors is not null)
                {
                    return new Response<string>("Can't created User right now.", resultUser.Errors.Select(s => s.Description).ToList());
                }

                return new Response<string>("Can't created User right now.");
            }
        }

        public async Task<Response<List<DropdownViewModel>>> ListAdminDropdown()
        {
            var Type = await Task.Run(() =>
            {
                var dropdownList = new List<DropdownViewModel>
                {
                    new DropdownViewModel
                    {
                        Id = 1,
                        Name = "Super Admin"
                    },
                    new DropdownViewModel
                    {
                        Id = 2,
                        Name = "Admin"
                    }
                };

                return dropdownList;
            });

            return new Response<List<DropdownViewModel>>(Type);
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response<bool>("User is not found.");
            }

            // Soft delete the user by setting the IsDeleted flag
            user.IsDeleted = true; // Assuming IsDeleted is a property in your user entity
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                // If soft deletion fails, handle the error accordingly
                return new Response<bool>(false, "Failed to delete user.");
            }

            return new Response<bool>(true);
        }

    }
}
