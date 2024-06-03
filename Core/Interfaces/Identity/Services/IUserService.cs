using Core.DTOs.Role;
using Core.DTOs.Shared;
using Core.DTOs.User;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Identity.Services
{
    public interface IUserService
    {
        Task<PagedResponse<List<UserDto>>> GetPagination(PaginationParameter filter);
        Task<PagedResponse<IReadOnlyList<ListAdminDto>>> GetAdminPagination(ListAdminParameters filter);
        Task<PagedResponse<IReadOnlyList<ListAdminDto>>> GetAudiencePagination(ListAdminParameters filter);
        Task<Response<bool>> ToggleStatus(ToggleStatusDto request);
        Task<Response<bool>> ToggleBlock(ToggleBlockDto request);
        Task<Response<List<DropdownViewModel>>> Dropdown();
        Task<Response<UserDto>> Details(string id);
        Task<Response<MobileUserDetailDto>> Profile();
        Task<Response<bool>> Update(UpdateUserDto request);
        Task<Response<bool>> Update(UserProfileDto request);
        Task<Response<List<RoleViewModel>>> AdminDropdown();
        Task<Response<string>> RegisterAdmin(UserAddModel request);
        Task<Response<List<DropdownViewModel>>> ListAdminDropdown();
        Task<Response<bool>> Delete(string userId);

    }
}
