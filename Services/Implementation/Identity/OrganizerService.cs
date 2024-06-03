using AutoMapper;
using Core;
using Core.DTOs.Event.Response;
using Core.DTOs.LookUps.Category.Response;
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
using System.Text.RegularExpressions;

namespace Services.Implementation.Identity
{
    internal sealed class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _organizerRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationHelperService _authHelperService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public OrganizerService(IOrganizerRepository organizerRepo,
                                IUnitOfWork unitOfWork,
                                RoleManager<Role> roleManager,
                                UserManager<User> userManager,
                                IAuthenticationHelperService authHelperService,
                                IDateTimeService dateTimeService,
                                ICategoryRepository categoryRepo,
                                IMapper mapper,
                                IAuthenticatedUserService authenticatedUserService)
        {
            _organizerRepo = organizerRepo;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _authHelperService = authHelperService;
            _dateTimeService = dateTimeService;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<PagedResponse<IList<ListEventOrganizerDto>>> GetPagination(EventOrganizerParameters filter)
        {
            var result = await _organizerRepo.GetPaginaton(filter);
            if (result.Count == 0 ||
                result.Data == null)
            {
                return new PagedResponse<IList<ListEventOrganizerDto>>(null, filter.PageNumber, filter.PageSize);
            }
            return new PagedResponse<IList<ListEventOrganizerDto>>(result.Data, filter.PageNumber, filter.PageSize, result.Count);
        }

        public async Task<Response<string>> Register(RegisterOrganizerDto request)
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

            if (request.Organizer is not null &&
                !await _categoryRepo.Exists(f => f.Id == request.Organizer.CategoryId))
            {
                return new Response<string>($"Category: {request.Organizer.CategoryId} not found.");
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
                    var organizer = _mapper.Map<Organizer>(request.Organizer);
                    organizer.UserId = user.Id;

                    _organizerRepo.Add(organizer);
                    await _unitOfWork.SaveAsync();

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

        public async Task<Response<IList<DropdownViewModel>>> Dropdown()
        {
            var categories = await _categoryRepo.GetAllWithSelectorAsync(s => new DropdownViewModel
            {
                Id = s.Id,
                Name = s.Name
            },
            true,
            null,
            o => o.OrderByDescending(x => x.Id));

            return new Response<IList<DropdownViewModel>>(categories);
        }

        public async Task<Response<OrganizerDetailDto>> Detail(long id)
        {
            var result = await _organizerRepo.GetPropertyWithSelectorAsync(s => new OrganizerDetailDto
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
                WebsiteURL = s.WebsiteURL,
                Address = s.Address,
                Description = s.Description,
                MapLink = s.MapLink
            }, true,
            f => f.Id == id && !f.IsDeleted);

            if (result is null)
            {
                return new Response<OrganizerDetailDto>(null, "No Data Found.");
            }

            return new Response<OrganizerDetailDto>(result!);
        }

        public async Task<Response<bool>> Update(EditOrganizerDto request)
        {
            var entity = await _organizerRepo.GetByIdAsync(request.Id);
            if (entity is null)
            {
                return new Response<bool>($"Organizer Id: {request.Id} not found.");
            }

            if (!await _categoryRepo.Exists(f => f.Id == request.CategoryId))
            {
                return new Response<bool>($"Category Id: {request.Id} not found.");
            }

            _mapper.Map(request, entity);

            _organizerRepo.Update(entity);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> Delete(long id)
        {
            if (!await _organizerRepo.Exists(f => f.Id == id))
            {
                return new Response<bool>($"Organizer Id: {id} not found.");
            }

            var entity = await _organizerRepo.GetByIdAsync(id);

            _organizerRepo.Remove(entity!);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }
    }
}
