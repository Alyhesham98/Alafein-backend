using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Identity.Services
{
    public interface IVenueService
    {
        Task<PagedResponse<IList<ListVenueDto>>> GetPagination(VenueParameters filter);
        Task<Response<string>> Register(RegisterVenueDto request);
        Task<Response<RegisterVenueDropdownDto>> Dropdown();
        Task<Response<VenueDetailDto>> Detail(long id);
    }
}
