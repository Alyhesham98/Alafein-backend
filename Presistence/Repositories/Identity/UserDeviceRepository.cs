using Core.DTOs.User.Request;
using Core.Entities.Identity;
using Core.Interfaces.Identity.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Identity
{
    internal sealed class UserDeviceRepository : GenericRepository<UserDevice>, IUserDeviceRepository
    {
        private readonly ApplicationDbContext _context;
        public UserDeviceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Update(UserDeviceDto device, string userId)
        {
            await _context.UserDevices
                          .Where(x => x.UserId == userId)
                          .ExecuteUpdateAsync(e => e.SetProperty(d => d.NotificationToken, device.NotificationToken));
            return true;
        }
    }
}
