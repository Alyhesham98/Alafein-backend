using Core.DTOs.Alert.Request;
using Core.Interfaces.Shared.Services;

namespace Core.CoreEvents.Notification
{
    public record PushNotificationCoreEvent(PushNotificationDto notification) : ICoreEvent;
}
