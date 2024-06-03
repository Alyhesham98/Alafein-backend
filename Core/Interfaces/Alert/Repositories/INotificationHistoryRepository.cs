using Core.Entities.Alert;
using Core.Interfaces.Base;

namespace Core.Interfaces.Alert.Repositories
{
    public interface INotificationHistoryRepository : IGenericRepository<NotificationHistory>
    {
        Task<bool> Read(long id);
    }
}
