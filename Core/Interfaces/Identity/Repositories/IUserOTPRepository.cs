using Core.Entities.Identity;
using Core.Interfaces.Base;

namespace Core.Interfaces.Identity.Repositories
{
    public interface IUserOTPRepository : IGenericRepository<UserOTP>
    {
        Task<bool> CheckOTP(string otp, string userId);
    }
}
