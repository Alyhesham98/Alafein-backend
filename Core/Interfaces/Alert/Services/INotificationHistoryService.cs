using Core.DTOs.Alert.Request;
using Core.DTOs.Alert.Response;
using Core.DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Alert.Services
{
    public interface INotificationHistoryService
    {
        Task<PagedResponse<IList<ListNotificationHistoryDto>>> GetPagination(PaginationParameter filter);
        Task<Response<bool>> Read(UpdateNotificationHistoryDto request);
    }
}
