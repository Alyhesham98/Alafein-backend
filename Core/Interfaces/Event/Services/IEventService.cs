using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Event.Services
{
    public interface IEventService
    {
        Task<PagedResponse<IList<ListEventMobileDto>>> GetMobilePagination(EventMobileListParameters filter);
        Task<PagedResponse<IList<ListEventDto>>> GetPagination(EventListParameters filter);
        Task<PagedResponse<IList<ListEventDto>>> GetPaginationSpotLight(EventListParameters filter);
        Task<Response<bool>> ToggleStatus(ToggleSubmissionStatusDto request);
        Task<Response<List<DropdownViewModel>>> Dropdown();
        Task<Response<bool>> ToggleSpotlight(ToggleSubmissionSpotlightDto request);
        Task<Response<EventDetailDto>> Detail(long id);
        Task<Response<HomeDto>> Home();
        Task<Response<bool>> ToggleFavourite(ToggleSubmissionFavouriteDto request);
        Task<Response<bool>> Comment(CommentDto request);
        Task<Response<FeeDto>> FeeConfiguration();
        Task<Response<bool>> SpotlightOrder(SpotlightOrderDto request);
    }
}
