using Core.Entities.Alert;
using Core.Interfaces.Alert.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Alert
{
    internal sealed class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
