using Core.DTOs.Alert.Request;
using Core.DTOs.Alert.Response;
using Core.DTOs.Shared;
using Core.Interfaces.Alert.Repositories;
using Core.Interfaces.Alert.Services;
using Core.Interfaces.Shared.Services;
using DTOs.Shared.Responses;

namespace Services.Implementation.Alert
{
    internal sealed class NotificationHistoryService : INotificationHistoryService
    {
        private readonly IAuthenticatedUserService _authenticatedService;
        private readonly INotificationHistoryRepository _notificationHistoryRepo;
        public NotificationHistoryService(IAuthenticatedUserService authenticatedService,
                                          INotificationHistoryRepository notificationHistoryRepo)
        {
            _authenticatedService = authenticatedService;
            _notificationHistoryRepo = notificationHistoryRepo;
        }

        public async Task<PagedResponse<IList<ListNotificationHistoryDto>>> GetPagination(PaginationParameter filter)
        {
            var pgTotal = await _notificationHistoryRepo.GetCountAsync(f => !f.IsDeleted &&
                                                                            f.IsSent &&
                                                                            f.UserId == _authenticatedService.UserId);

            var result = await _notificationHistoryRepo.GetPagedWithSelectorAsync(s => new ListNotificationHistoryDto
            {
                Id = s.Id,
                IsRead = s.IsRead,
                Body = s.Notification.Body,
                Title = s.Notification.Title,
            },
            filter.PageNumber,
            filter.PageSize,
            true,
            f => f.IsSent &&
                 f.UserId == _authenticatedService.UserId);

            return new PagedResponse<IList<ListNotificationHistoryDto>>(result, filter.PageNumber, filter.PageSize, pgTotal);
        }

        public async Task<Response<bool>> Read(UpdateNotificationHistoryDto request)
        {
            var entity = await _notificationHistoryRepo.GetByIdAsync(request.Id,
                                                                     f => f.IsSent);
            if (entity is null)
            {
                return new Response<bool>("Notification Id not found.");
            }

            if (entity.IsRead)
            {
                return new Response<bool>("Notification already has beed read.");
            }

            await _notificationHistoryRepo.Read(request.Id);
            return new Response<bool>(true);
        }
    }
}
