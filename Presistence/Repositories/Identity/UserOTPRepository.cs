using Core.Entities.Identity;
using Core.Interfaces.Identity.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Identity
{
    internal sealed class UserOTPRepository : GenericRepository<UserOTP>, IUserOTPRepository
    {
        private readonly ApplicationDbContext _context;
        public UserOTPRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CheckOTP(string otp, string userId)
        {
            var lastOTP = await _context.UserOTPs
                                        .Where(f => f.UserId == userId)
                                        .OrderByDescending(o => o.Id)
                                        .FirstOrDefaultAsync();
            if (lastOTP is not null &&
                lastOTP.OTP == otp)
            {
                return true;
            }
            return false;
        }
    }
}
