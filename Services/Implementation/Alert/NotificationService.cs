using AutoMapper;
using Core;
using Core.CoreEvents.Notification;
using Core.DTOs.Alert.Request;
using Core.DTOs.Alert.Response;
using Core.DTOs.Shared;
using Core.Entities.Alert;
using Core.Interfaces.Alert.Repositories;
using Core.Interfaces.Alert.Services;
using Core.Interfaces.Shared.Services;
using DTOs.Shared.Responses;
using Services.Implementation.Shared;

namespace Services.Implementation.Alert
{
    internal sealed class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IAuthenticatedUserService _authenticatedService;
        public NotificationService(INotificationRepository notificationRepo,
                                   IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   IBackgroundJobService backgroundJobService,
                                   IAuthenticatedUserService authenticatedService)
        {
            _notificationRepo = notificationRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _backgroundJobService = backgroundJobService;
            _authenticatedService = authenticatedService;
        }

        public async Task<Response<long>> Add(AddNotificationDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Body) ||
                string.IsNullOrWhiteSpace(request.BodyAr) ||
                string.IsNullOrWhiteSpace(request.Title) ||
                string.IsNullOrWhiteSpace(request.TitleAr))
            {
                return new Response<long>("Notification body and title cannot be empty.");
            }
            var result = _notificationRepo.Add(_mapper.Map<Notification>(request));
            await _unitOfWork.SaveAsync();

            _backgroundJobService.Enqueue<ProcessCoreEventJob>(e => e.Process(
                                                                                new PushNotificationCoreEvent(new PushNotificationDto
                                                                                {
                                                                                    Id = result.Id,
                                                                                    Body = result.Body,
                                                                                    Title = result.Title,
                                                                                    CreatedBy = _authenticatedService.UserId!
                                                                                })));

            return new Response<long>(result.Id);
        }

        public async Task<Response<bool>> Update(UpdateNotificationDto request)
        {
            var entity = await _notificationRepo.GetByIdAsync(request.Id);
            if (entity is null)
            {
                return new Response<bool>("Notification Id not found.");
            }

            _mapper.Map(request, entity);

            _notificationRepo.Update(entity);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> Delete(long id)
        {
            if (!await _notificationRepo.Exists(f => f.Id == id))
            {
                return new Response<bool>("Category Id not found.");
            }

            var entity = await _notificationRepo.GetByIdAsync(id);

            _notificationRepo.Remove(entity!);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<PagedResponse<IList<ListNotificationDto>>> GetPagination(PaginationParameter filter, bool isAscending = false)
        {
            var pgTotal = await _notificationRepo.GetCountAsync(f => !f.IsDeleted);

            var result = await _notificationRepo.GetPagedWithSelectorAsync(s => new ListNotificationDto
            {
                Id = s.Id,
                Body = s.Body,
                BodyAr = s.BodyAr,
                Title = s.Title,
                TitleAr = s.TitleAr,
                Schedule = s.Schedule
            },
            filter.PageNumber,
            filter.PageSize,
            true,
            null,
            isAscending ? o => o.OrderBy(x => x.Id) :
                          o => o.OrderByDescending(x => x.Id));

            return new PagedResponse<IList<ListNotificationDto>>(result, filter.PageNumber, filter.PageSize, pgTotal);
        }

        public async Task<Response<IList<ListNotificationDto>>> GetWithoutPagination(bool isAscending = false)
        {
            var result = await _notificationRepo.GetAllWithSelectorAsync(s => new ListNotificationDto
            {
                Id = s.Id,
                Body = s.Body,
                BodyAr = s.BodyAr,
                Title = s.Title,
                TitleAr = s.TitleAr,
                Schedule = s.Schedule
            },
            true,
            null,
            isAscending ? o => o.OrderBy(x => x.Id) :
                          o => o.OrderByDescending(x => x.Id));

            return new Response<IList<ListNotificationDto>>(result);
        }
    }
}
