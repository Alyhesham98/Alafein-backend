using Core.DTOs.User.Request;
using Core.Entities.Identity;
using Core.Interfaces.Base;

namespace Core.Interfaces.Identity.Repositories
{
    public interface IUserDeviceRepository : IGenericRepository<UserDevice>
    {
        Task<bool> Update(UserDeviceDto device, string userId);
    }
}
