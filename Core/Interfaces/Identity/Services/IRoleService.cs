using Core.DTOs.Role;
using Core.DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Identity.Services
{
    public interface IRoleService
    {
        Task<Response<RoleViewModel>> CreateRoleAsync(RoleAddModel request);
        Task<Response<RoleViewModel>> UpdateRoleAsync(RoleViewModel request);
        Task<Response<bool>> DeleteRoleAsync(string Id);
        Task<Response<RoleViewModel>> GetRoleByIdAsync(string Id);
        Task<PagedResponse<List<RoleViewModel>>> GetRolesWithPaginationAsync(PaginationParameter request);
        Task<Response<List<RoleViewModel>>> GetRolesWithoutPaginationAsync();
        Task<Response<List<RoleViewModel>>> GetRegisterRoles();
    }
}
