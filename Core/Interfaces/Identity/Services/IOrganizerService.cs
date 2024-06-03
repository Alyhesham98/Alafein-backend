using Core.DTOs.Event.Response;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Identity.Services
{
    public interface IOrganizerService
    {
        Task<PagedResponse<IList<ListEventOrganizerDto>>> GetPagination(EventOrganizerParameters filter);
        Task<Response<string>> Register(RegisterOrganizerDto request);
        Task<Response<IList<DropdownViewModel>>> Dropdown();
        Task<Response<OrganizerDetailDto>> Detail(long id);
        Task<Response<bool>> Update(EditOrganizerDto request);
        Task<Response<bool>> Delete(long id);
    }
}
