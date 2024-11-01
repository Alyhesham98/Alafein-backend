using AutoMapper;
using Core;
using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.Role;
using Core.DTOs.Shared.Email;
using Core.DTOs.User;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Entities.Identity;
using Core.Enums;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.Identity.Repositories;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using DTOs.Shared;
using DTOs.Shared.Responses;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Services.Implementation.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationHelperService _authHelperService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IFacilityRepository _facilityRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrganizerRepository _organizerRepo;
        private readonly IVenueRepository _venueRepo;
        private readonly JWTSettings _jwtSettings;
        private readonly IUserOTPRepository _userOTPRepo;
        private readonly ISendgridService _sendgridService;
        private readonly IEmailRenderService _emailRenderService;
        private readonly IUserDeviceRepository _userDeviceRepo;
        public AuthenticationService(RoleManager<Role> roleManager,
                                     UserManager<User> userManager,
                                     IAuthenticationHelperService authHelperService,
                                     IDateTimeService dateTimeService,
                                     ICategoryRepository categoryRepo,
                                     IFacilityRepository facilityRepo,
                                     IUnitOfWork unitOfWork,
                                     IOptions<JWTSettings> jwtSettings,
                                     IMapper mapper,
                                     IVenueRepository venueRepo,
                                     IOrganizerRepository organizerRepo,
                                     IUserOTPRepository userOTPRepo,
                                     ISendgridService sendgridService,
                                     IEmailRenderService emailRenderService,
                                     IUserDeviceRepository userDeviceRepo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _authHelperService = authHelperService;
            _dateTimeService = dateTimeService;
            _facilityRepo = facilityRepo;
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            _venueRepo = venueRepo;
            _organizerRepo = organizerRepo;
            _userOTPRepo = userOTPRepo;
            _sendgridService = sendgridService;
            _emailRenderService = emailRenderService;
            _userDeviceRepo = userDeviceRepo;
        }

        public async Task<Response<bool>> SendOtp(SendOtpDto request)
        {
            var user = await _userManager.Users
                                         .AnyAsync(f => f.Email == request.Email);
            if (!user)
            {
                return new Response<bool>("Email not found.");
            }

            var userId = await _userManager.Users
                                           .Where(f => f.Email == request.Email)
                                           .Select(s => s.Id)
                                           .FirstOrDefaultAsync();

            var count = await _userOTPRepo.GetCountAsync(f => f.UserId == userId &&
                                                              f.CreatedAt
                                                               .Date == _dateTimeService.NowUtc
                                                                                        .Date);
            if (count == 3)
            {
                return new Response<bool>("Max number of otp sent contact admin.");
            }

            var otp = _authHelperService.GetOTP(6);

            var content = _emailRenderService.RenderLiquidTemplate("OtpEmail.html", new RenderOtpEmailModel
            {
                Code = otp
            });

            var response = await _sendgridService.SendEmail(new EmailModel
            {
                Body = content,
                Subject = "OTP verification",
                To = request.Email
            });

            if (!response.Succeeded)
            {
                return new Response<bool>("Error when update latest employee login.");
            }

            _userOTPRepo.Add(new UserOTP
            {
                OTP = otp,
                UserId = userId!,
                CreatedAt = _dateTimeService.NowUtc,
                CreatedBy = "AlaFein System"
            });
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true, "OTP sent");
        }

        public async Task<Response<HomeScreenModel>> ValidateOtp(VerifyOtpDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new Response<HomeScreenModel>($"No accounts registered with {request.Email}.");
            }

            var lastOTP = await _userOTPRepo.CheckOTP(request.OTP, user.Id);

            if (!lastOTP)
            {
                return new Response<HomeScreenModel>($"Invalid otp for: {request.Email}.");
            }

            var userSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);

            if (userSecurityStamp.Succeeded == true)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles is null ||
                    roles.Count == 0)
                {
                    return new Response<HomeScreenModel>("Invalid Credentials.");
                }

                var response = new HomeScreenModel
                {
                    Name = user.FirstName,
                    Email = user.Email!,
                    JWTToken = await _authHelperService.generateJwtToken(user.Id, roles.FirstOrDefault()!, user.SecurityStamp!),
                    TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                    Role = roles.FirstOrDefault()!,
                    EmailVerified = user.EmailConfirmed,
                    ProfileImage = user.ProfilePicture
                };


                return new Response<HomeScreenModel>(response);
            }
            return new Response<HomeScreenModel>("Error when update latest employee login.");
        }

        public async Task<Response<HomeScreenModel>> Login(LoginModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new Response<HomeScreenModel>($"No accounts registered with {request.Email}.");
            }

            if (user.IsDeleted)
            {
                return new Response<HomeScreenModel>($"Your account was deleted.");
            }
            var result = _authHelperService.VerifyPasswordHash(request.Password, user.PasswordHash!, user.PasswordSalt);

            if (!result)
            {
                return new Response<HomeScreenModel>($"Invalid credentials for: {request.Email}.");
            }

            if (!user.EmailConfirmed)
            {
                return new Response<HomeScreenModel>($"Account not verified yet for: {request.Email}.");
            }

            var userSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);

            if (userSecurityStamp.Succeeded == true)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles is null || roles.Count == 0)
                {
                    return new Response<HomeScreenModel>("Invalid Credentials.");
                }

                var response = new HomeScreenModel
                {
                    Name = user.FirstName,
                    Email = user.Email!,
                    JWTToken = await _authHelperService.generateJwtToken(user.Id, roles.FirstOrDefault()!, user.SecurityStamp!),
                    TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                    Role = roles.FirstOrDefault()!,
                    EmailVerified = user.EmailConfirmed,
                    ProfileImage = user.ProfilePicture
                };


                return new Response<HomeScreenModel>(response);
            }
            return new Response<HomeScreenModel>("Error when update latest employee login.");
        }

        public async Task<Response<HomeScreenModel>> MobileLogin(MobileLoginModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new Response<HomeScreenModel>($"No accounts registered with {request.Email}.");
            }

            if (user is not null &&
           user.IsDeleted)
            {
                return new Response<HomeScreenModel>($"Your Account wad deleted.");
            }

            if (user is not null &&
                user.IsBlocked)
            {
                return new Response<HomeScreenModel>($"You are not verified.");
            }

            var result = _authHelperService.VerifyPasswordHash(request.Password, user!.PasswordHash!, user.PasswordSalt);

            if (!result)
            {
                return new Response<HomeScreenModel>($"Invalid credentials for: {request.Email}.");
            }

            if (!user.EmailConfirmed)
            {
                return new Response<HomeScreenModel>($"Account not verified yet for: {request.Email}.");
            }

            var userSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);

            if (userSecurityStamp.Succeeded == true)
            {
                if (request.Device is not null &&
                    await _userDeviceRepo.Exists(f => f.UserId == user.Id))
                {
                    await _userDeviceRepo.Update(request.Device, user.Id);
                }
                else if (request.Device is not null &&
                         !await _userDeviceRepo.Exists(f => f.UserId == user.Id))
                {
                    _userDeviceRepo.Add(new UserDevice
                    {
                        UserId = user.Id,
                        NotificationToken = request.Device.NotificationToken
                    });

                    await _unitOfWork.SaveAsync();
                }
                var roles = await _userManager.GetRolesAsync(user);

                if (roles is null || roles.Count == 0)
                {
                    return new Response<HomeScreenModel>("Invalid Credentials.");
                }

                var response = new HomeScreenModel
                {
                    Name = user.FirstName,
                    Email = user.Email!,
                    JWTToken = await _authHelperService.generateJwtToken(user.Id, roles.FirstOrDefault()!, user.SecurityStamp!),
                    TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                    Role = roles.FirstOrDefault()!,
                    EmailVerified = user.EmailConfirmed,
                    ProfileImage = user.ProfilePicture
                };


                return new Response<HomeScreenModel>(response);
            }
            return new Response<HomeScreenModel>("Error when update latest employee login.");
        }

        public async Task<Response<HomeScreenModel>> RefreshToken(RefreshTokenModel request)
        {
            var handler = new JwtSecurityTokenHandler();
            var JwtSecurityToken = handler.ReadToken(request.Token.Replace("Bearer ", "")) as JwtSecurityToken;

            var userId = JwtSecurityToken!.Claims.First(claim => claim.Type == "uid").Value;
            if (userId is null)
            {
                return new Response<HomeScreenModel>($"Log Out");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null ||
                !user.EmailConfirmed)
            {
                return new Response<HomeScreenModel>($"Log Out");
            }
            if (user is not null &&
                user.IsBlocked)
            {
                return new Response<HomeScreenModel>($"Log Out, user blocked.");
            }

            var roles = await _userManager.GetRolesAsync(user!);
            return new Response<HomeScreenModel>(new HomeScreenModel
            {
                Name = user!.FirstName + " " + user.LastName,
                Email = user.Email!,
                JWTToken = await _authHelperService.generateJwtToken(user.Id, roles.FirstOrDefault()!, user.SecurityStamp!),
                TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                Role = roles.FirstOrDefault()!,
                EmailVerified = user.EmailConfirmed,
                ProfileImage = user.ProfilePicture
            });
        }

        public async Task<Response<HomeScreenModel>> GoogleAuth(ExternalLoginViewModel request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.AccessToken, new GoogleJsonWebSignature.ValidationSettings());
            if (payload is null)
            {
                return new Response<HomeScreenModel>("Invalid Authentication.");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                string[] emailSplit = payload.Email.Split("@");
                var audience = new User
                {
                    Email = payload.Email,
                    NormalizedEmail = payload.Email.ToUpper(),
                    UserName = emailSplit[0],
                    NormalizedUserName = emailSplit[0].ToUpper(),
                    CreatedBy = "GoogleProvider",
                    CreatedAt = _dateTimeService.NowUtc,
                    Status = (int)Status.Normal,
                    IsBlocked = false,
                    PhoneNumberConfirmed = false,
                    PhoneNumber = null,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    EmailConfirmed = true,
                    ProfilePicture = null,
                };
                var resultUser = await _userManager.CreateAsync(audience);

                await _userManager.AddLoginAsync(audience, new UserLoginInfo("Google", payload.Subject, payload.Name));

                await _userManager.AddToRoleAsync(audience, "Audience");

                if (request.Device is not null &&
                    await _userDeviceRepo.Exists(f => f.UserId == audience.Id))
                {
                    await _userDeviceRepo.Update(request.Device, audience.Id);
                }
                else
                {
                    _userDeviceRepo.Add(new UserDevice
                    {
                        UserId = audience.Id,
                        NotificationToken = request.AccessToken
                    });

                    await _unitOfWork.SaveAsync();
                }

                var rolesAud = await _userManager.GetRolesAsync(audience);

                if (rolesAud is null || rolesAud.Count == 0)
                {
                    return new Response<HomeScreenModel>("Invalid Credentials.");
                }
                return new Response<HomeScreenModel>(new HomeScreenModel
                {
                    Name = audience.FirstName,
                    Email = audience.Email!,
                    JWTToken = await _authHelperService.generateJwtToken(audience.Id, rolesAud.FirstOrDefault()!, audience.SecurityStamp!),
                    TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                    Role = rolesAud.FirstOrDefault()!,
                    EmailVerified = audience.EmailConfirmed,
                    ProfileImage = audience.ProfilePicture
                });
            }

            if (request.Device is not null &&
                await _userDeviceRepo.Exists(f => f.UserId == user.Id))
            {
                await _userDeviceRepo.Update(request.Device, user.Id);
            }
            else
            {
                _userDeviceRepo.Add(new UserDevice
                {
                    UserId = user.Id,
                    NotificationToken = request.AccessToken
                });

                await _unitOfWork.SaveAsync();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles is null || roles.Count == 0)
            {
                return new Response<HomeScreenModel>("Invalid Credentials.");
            }

            var response = new HomeScreenModel
            {
                Name = user.FirstName,
                Email = user.Email!,
                JWTToken = await _authHelperService.generateJwtToken(user.Id, roles.FirstOrDefault()!, user.SecurityStamp!),
                TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                Role = roles.FirstOrDefault()!,
                EmailVerified = user.EmailConfirmed,
                ProfileImage = user.ProfilePicture
            };
            return new Response<HomeScreenModel>(response);
        }

        public async Task<Response<HomeScreenModel>> FacebookAuth(ExternalLoginViewModel request)
        {
            // verify access token with facebook API to authenticate
            var client = new RestClient("https://graph.facebook.com/v19.0");
            var requestContainer = new RestRequest($"me?fields=id%2Cfirst_name%2Clast_name%2Cemail&access_token={request.AccessToken}");

            var FacebookResponse = await client.GetAsync(requestContainer);
            if (!FacebookResponse.IsSuccessful)
            {
                return new Response<HomeScreenModel>("Invalid Authentication.");
            }

            var payload = JsonSerializer.Deserialize<FacebookAuthenticationPayload>(FacebookResponse.Content!);

            var user = await _userManager.FindByEmailAsync(payload!.Email);
            if (user is null)
            {
                string[] emailSplit = payload.Email.Split("@");
                var audience = new User
                {
                    Email = payload.Email,
                    NormalizedEmail = payload.Email.ToUpper(),
                    UserName = emailSplit[0],
                    NormalizedUserName = emailSplit[0].ToUpper(),
                    CreatedBy = "FacebookProvider",
                    CreatedAt = _dateTimeService.NowUtc,
                    Status = (int)Status.Normal,
                    IsBlocked = false,
                    PhoneNumberConfirmed = false,
                    PhoneNumber = null,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    EmailConfirmed = true,
                    ProfilePicture = null,
                };
                var resultUser = await _userManager.CreateAsync(audience);

                await _userManager.AddLoginAsync(audience, new UserLoginInfo("Facebook", payload.Id, payload.Name));

                await _userManager.AddToRoleAsync(audience, "Audience");

                if (request.Device is not null &&
                    await _userDeviceRepo.Exists(f => f.UserId == audience.Id))
                {
                    await _userDeviceRepo.Update(request.Device, audience.Id);
                }
                else
                {
                    _userDeviceRepo.Add(new UserDevice
                    {
                        UserId = audience.Id,
                        NotificationToken = request.AccessToken
                    });

                    await _unitOfWork.SaveAsync();
                }

                var rolesAud = await _userManager.GetRolesAsync(audience);

                if (rolesAud is null || rolesAud.Count == 0)
                {
                    return new Response<HomeScreenModel>("Invalid Credentials.");
                }
                return new Response<HomeScreenModel>(new HomeScreenModel
                {
                    Name = audience.FirstName,
                    Email = audience.Email!,
                    JWTToken = await _authHelperService.generateJwtToken(audience.Id, rolesAud.FirstOrDefault()!, audience.SecurityStamp!),
                    TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                    Role = rolesAud.FirstOrDefault()!,
                    EmailVerified = audience.EmailConfirmed,
                    ProfileImage = audience.ProfilePicture
                });
            }

            if (request.Device is not null &&
                await _userDeviceRepo.Exists(f => f.UserId == user.Id))
            {
                await _userDeviceRepo.Update(request.Device, user.Id);
            }
            else
            {
                _userDeviceRepo.Add(new UserDevice
                {
                    UserId = user.Id,
                    NotificationToken = request.AccessToken
                });

                await _unitOfWork.SaveAsync();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles is null || roles.Count == 0)
            {
                return new Response<HomeScreenModel>("Invalid Credentials.");
            }

            var response = new HomeScreenModel
            {
                Name = user.FirstName,
                Email = user.Email!,
                JWTToken = await _authHelperService.generateJwtToken(user.Id, roles.FirstOrDefault()!, user.SecurityStamp!),
                TokenExpiration = _dateTimeService.NowUtc.AddDays(_jwtSettings.DurationInDays),
                Role = roles.FirstOrDefault()!,
                EmailVerified = user.EmailConfirmed,
                ProfileImage = user.ProfilePicture
            };
            return new Response<HomeScreenModel>(response);
        }

        public async Task<Response<string>> Register(RegisterDto request)
        {
            #region Validation
            if (!Regex.IsMatch(request.User.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!”#$%&’()*+,-./:;<=>?@[\]^_`{|}~']).{8,}$"))
            {
                return new Response<string>($"Password format should contain At Least Upper Case letter, lower Case letter, Special Character, and Number.");
            }

            var role = await _roleManager.FindByIdAsync(request.User.RoleId);
            if (role is null)
            {
                return new Response<string>($"Role not exist.");
            }

            if (request.Organizer is not null &&
                !await _categoryRepo.Exists(f => f.Id == request.Organizer.CategoryId))
            {
                return new Response<string>($"Category: {request.Organizer.CategoryId} not found.");
            }
            if (request.Venue is not null &&
                !await _categoryRepo.Exists(f => f.Id == request.Venue.CategoryId))
            {
                return new Response<string>($"Category: {request.Venue.CategoryId} not found.");
            }
            #endregion

            var user = _mapper.Map<User>(request.User);

            #region Filling data
            user.CreatedBy = "System";
            user.CreatedAt = _dateTimeService.NowUtc;
            string[] emailSplit = user.Email!.Split("@");
            user.UserName = emailSplit[0];
            user.NormalizedUserName = emailSplit[0].ToUpper();
            _authHelperService.CreatePasswordHash(request.User.Password, out string passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            #endregion
            if (request.Organizer is not null || request.Venue is not null)
            {
                user.IsBlocked = true;
            }
            var resultUser = await _userManager.CreateAsync(user);
            if (resultUser.Succeeded)
            {
                var resultUserRole = await _userManager.AddToRoleAsync(user, role.Name!);
                /*
                 * var verificationMail = await _authHelperService.SendVerificationMail(name: user.Name,
                                                                                     email: user.Email!,
                                                                                     id: user.Id,
                                                                                     token: user.PasswordResetToken);

                if (!verificationMail.Succeeded)
                {
                    return new Response<string>(verificationMail.Message);
                }
                */

                if (resultUserRole.Succeeded)
                {
                    if (request.Organizer is not null)
                    {
                        var organizer = _mapper.Map<Organizer>(request.Organizer);
                        organizer.UserId = user.Id;

                        _organizerRepo.Add(organizer);
                        await _unitOfWork.SaveAsync();
                    }
                    else if (request.Venue is not null)
                    {
                        var venue = _mapper.Map<Venue>(request.Venue);
                        venue.UserId = user.Id;

                        _venueRepo.Add(venue);
                        await _unitOfWork.SaveAsync();
                    }
                    return new Response<string>(user.Id, "Client created successfully");
                }
                return new Response<string>("This email is already associated with an existing account. Please log in or use a different email.", resultUserRole.Errors.Select(s => s.Description).ToList());
            }
            else
            {
                if (resultUser.Errors is not null)
                {
                    return new Response<string>("This email is already associated with an existing account. Please log in or use a different email.", resultUser.Errors.Select(s => s.Description).ToList());
                }

                return new Response<string>("This email is already associated with an existing account. Please log in or use a different email.");
            }
        }

        public async Task<Response<RegisterDropdownDto>> Dropdown()
        {
            var roles = await _roleManager.Roles
                                          .Where(f => f.NormalizedName == "HOST VENUE" ||
                                                      f.NormalizedName == "AUDIENCE")
                                          .Select(c => new RoleViewModel
                                          {
                                              Id = c.Id,
                                              Name = c.Name!
                                          })
                                          .OrderBy(x => x.Name)
                                          .AsNoTracking()
                                          .ToListAsync();

            var facilities = await _facilityRepo.GetAllWithSelectorAsync(s => new ListFacilityDto
            {
                Id = s.Id,
                ImageName = s.ImageName,
                ImagePath = s.ImagePath
            },
            true,
            null,
            o => o.OrderByDescending(x => x.Id));

            var categories = await _categoryRepo.GetAllWithSelectorAsync(s => new DropdownViewModel
            {
                Id = s.Id,
                Name = s.Name
            },
            true,
            null,
            o => o.OrderByDescending(x => x.Id));

            var days = DayDropdown();

            return new Response<RegisterDropdownDto>(new RegisterDropdownDto
            {
                Category = categories,
                Facility = facilities,
                Roles = roles,
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
    }
}