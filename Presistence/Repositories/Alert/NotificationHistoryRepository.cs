using Core.Entities.Alert;
using Core.Interfaces.Alert.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Alert
{
    internal sealed class NotificationHistoryRepository : GenericRepository<NotificationHistory>, INotificationHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationHistoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Read(long id)
        {
            await _context.NotificationsHistory
                          .Where(x => x.Id == id)
                          .ExecuteUpdateAsync(e => e.SetProperty(d => d.IsRead, true));
            return true;
        }
    }
}
