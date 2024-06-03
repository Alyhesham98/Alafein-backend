using Core.DTOs.Alert.Request;
using Core.DTOs.Alert.Response;
using Core.DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Alert.Services
{
    public interface INotificationService
    {
        Task<Response<long>> Add(AddNotificationDto request);
        Task<Response<bool>> Update(UpdateNotificationDto request);
        Task<Response<bool>> Delete(long id);
        Task<PagedResponse<IList<ListNotificationDto>>> GetPagination(PaginationParameter filter, bool isAscending = false);
        Task<Response<IList<ListNotificationDto>>> GetWithoutPagination(bool isAscending = false);
    }
}
