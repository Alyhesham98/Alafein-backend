using Core;
using Core.CoreEvents.Notification;
using Core.DTOs.User.Request;
using Core.Entities.Alert;
using Core.Interfaces.Alert.Repositories;
using Core.Interfaces.Identity.Repositories;
using Core.Interfaces.Shared.Services;
using FirebaseAdmin.Messaging;
using MediatR;

namespace Services.EventHandlers.Notification
{
    internal sealed class PushNotificationCoreEventHandler : INotificationHandler<PushNotificationCoreEvent>
    {
        private readonly IUserDeviceRepository _userDeviceRepo;
        private readonly INotificationHistoryRepository _notificationHistoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeService _dateTimeService;
        public PushNotificationCoreEventHandler(IUserDeviceRepository userDeviceRepo,
                                                INotificationHistoryRepository notificationHistoryRepo,
                                                IUnitOfWork unitOfWork,
                                                IDateTimeService dateTimeService)
        {
            _userDeviceRepo = userDeviceRepo;
            _notificationHistoryRepo = notificationHistoryRepo;
            _unitOfWork = unitOfWork;
            _dateTimeService = dateTimeService;
        }

        public async Task Handle(PushNotificationCoreEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                return;
            }

            var userDevices = await _userDeviceRepo.GetAllWithSelectorAsync(s => new UserDeviceNotificationDto
            {
                NotificationToken = s.NotificationToken,
                UserId = s.UserId,
            },
            true,
            f => !f.IsDeleted &&
            !string.IsNullOrEmpty(f.NotificationToken));

            foreach (var userDevice in userDevices)
            {
                var message = new Message
                {
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notification.notification.Title,
                        Body = notification.notification.Body,
                    },
                    Token = userDevice.NotificationToken
                };

                var notificationHistory = new NotificationHistory
                {
                    NotificationId = notification.notification.Id,
                    UserId = userDevice.UserId,
                    IsRead = false,
                    IsSent = false,
                    CreatedAt = _dateTimeService.NowUtc,
                    CreatedBy = notification.notification.CreatedBy,
                };

                try
                {
                    var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                    notificationHistory.IsSent = true;
                    _notificationHistoryRepo.Add(notificationHistory);
                    await _unitOfWork.SaveAsync();
                }
                catch (FirebaseAdmin.Messaging.FirebaseMessagingException ex)
                {
                    notificationHistory.IsSent = false;
                    _notificationHistoryRepo.Add(notificationHistory);
                    await _unitOfWork.SaveAsync();

                    Console.WriteLine($"Failed to send notification to token {userDevice.NotificationToken}: {ex.Message}");
                    continue;
                }
                catch (Exception ex)
                {
                    notificationHistory.IsSent = false;
                    _notificationHistoryRepo.Add(notificationHistory);
                    await _unitOfWork.SaveAsync();

                    Console.WriteLine($"Failed to send notification to token {userDevice.NotificationToken}: {ex.Message}");
                    continue;
                }
            }
        }
    }
}
