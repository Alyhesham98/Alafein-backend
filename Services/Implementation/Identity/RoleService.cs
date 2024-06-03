using AutoMapper;
using Core.DTOs.Role;
using Core.DTOs.Shared;
using Core.Entities.Identity;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.Shared.Services;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementation.Identity
{
    internal sealed class RoleService : IRoleService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        public RoleService(IAuthenticatedUserService authenticatedUserService,
                           IDateTimeService dateTimeService,
                           RoleManager<Role> roleManager,
                           IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dateTimeService = dateTimeService;
            _authenticatedUserService = authenticatedUserService;
            _roleManager = roleManager;
        }

        public async Task<Response<RoleViewModel>> CreateRoleAsync(RoleAddModel request)
        {
            if (string.IsNullOrWhiteSpace(_authenticatedUserService.UserId))
            {
                return new Response<RoleViewModel>("User is not authorized.");
            }

            var roleCheck = await _roleManager.FindByNameAsync(request.Name);
            if (roleCheck is not null)
            {
                return new Response<RoleViewModel>($"Role: {request.Name} is already exist.");
            }

            var role = _mapper.Map<Role>(request);
            role.CreatedBy = _authenticatedUserService.UserId;
            role.CreatedAt = _dateTimeService.NowUtc;
            await _roleManager.CreateAsync(role);

            return new Response<RoleViewModel>(_mapper.Map<RoleViewModel>(role), $"Role: {role.Name} Created Successfully.");
        }

        public async Task<Response<RoleViewModel>> UpdateRoleAsync(RoleViewModel request)
        {
            if (string.IsNullOrWhiteSpace(_authenticatedUserService.UserId))
            {
                return new Response<RoleViewModel>("User is not authorized.");
            }

            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role is null)
            {
                return new Response<RoleViewModel>($"Role: {request.Name} is not exist.");
            }

            #region UpdateFields
            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpper();
            role.LastModifiedBy = _authenticatedUserService.UserId;
            role.LastModifiedAt = _dateTimeService.NowUtc;
            #endregion

            await _roleManager.UpdateAsync(role);

            return new Response<RoleViewModel>(_mapper.Map<RoleViewModel>(role), $"Role: {request.Name} Updated Successfully.");
        }

        public async Task<Response<bool>> DeleteRoleAsync(string roleId)
        {
            if (string.IsNullOrWhiteSpace(_authenticatedUserService.UserId))
            {
                return new Response<bool>("User is not authorized.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is not null)
            {
                return new Response<bool>($"Role: {role.Name} is not exist.");
            }

            await _roleManager.DeleteAsync(role);
            return new Response<bool>(true, $"Role: {role.Name} Deleted Successfully.");
        }

        public async Task<Response<RoleViewModel>> GetRoleByIdAsync(string roleId)
        {
            if (string.IsNullOrWhiteSpace(_authenticatedUserService.UserId))
            {
                return new Response<RoleViewModel>("User is not authorized.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is not null)
            {
                var result = _mapper.Map<RoleViewModel>(role);
                return new Response<RoleViewModel>(result);
            }

            return new Response<RoleViewModel>($"Role Id: {roleId} is not exist.");
        }

        public async Task<PagedResponse<List<RoleViewModel>>> GetRolesWithPaginationAsync(PaginationParameter request)
        {
            if (string.IsNullOrWhiteSpace(_authenticatedUserService.UserId))
            {
                return new PagedResponse<List<RoleViewModel>>("User is not authorized.");
            }

            var rolesCount = await _roleManager.Roles
                                               .CountAsync();

            var roles = await _roleManager.Roles
                                          .Select(c => new RoleViewModel
                                          {
                                              Id = c.Id,
                                              Name = c.Name!
                                          })
                                          .Skip((request.PageNumber - 1) * request.PageSize)
                                          .Take(request.PageSize)
                                          .OrderBy(x => x.Name)
                                          .AsNoTracking()
                                          .ToListAsync();


            if (roles is null || rolesCount > 0)
            {
                return new PagedResponse<List<RoleViewModel>>(roles!, request.PageNumber, request.PageSize, rolesCount);
            }

            return new PagedResponse<List<RoleViewModel>>(null, request.PageNumber, request.PageSize);
        }

        public async Task<Response<List<RoleViewModel>>> GetRolesWithoutPaginationAsync()
        {
            if (string.IsNullOrWhiteSpace(_authenticatedUserService.UserId))
            {
                return new Response<List<RoleViewModel>>("User is not authorized.");
            }

            var roles = await _roleManager.Roles
                                          .Select(c => new RoleViewModel
                                          {
                                              Id = c.Id,
                                              Name = c.Name!
                                          })
                                          .OrderBy(x => x.Name)
                                          .AsNoTracking()
                                          .ToListAsync();

            if (roles is null || roles.Count > 0)
            {
                return new Response<List<RoleViewModel>>(roles!);
            }
            return new Response<List<RoleViewModel>>(null, "No Data Found.");
        }

        public async Task<Response<List<RoleViewModel>>> GetRegisterRoles()
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

            return new Response<List<RoleViewModel>>(roles);
        }
    }
}
