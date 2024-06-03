using Core;
using Core.Entities.Identity;
using Core.Enums;
using Core.Interfaces.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementation.Identity
{
    internal sealed class SeedSuperAdminDataService : ISeedSuperAdminDataService
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationHelperService _authHelperService;
        private readonly IUnitOfWork _unitOfWork;
        public SeedSuperAdminDataService(UserManager<User> userManager,
                                         IAuthenticationHelperService authHelperService,
                                         IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _authHelperService = authHelperService;
            _unitOfWork = unitOfWork;
        }

        public async Task SeedAdminAsync()
        {
            if (await _userManager.Users
                                  .AnyAsync(f => f.NormalizedEmail == "SUPERADMIN@ALAFEIN.COM"))
            {
                return;
            }

            _authHelperService.CreatePasswordHash("123Pa$$word!", out string passwordHash, out byte[] passwordSalt);

            var defaultUser = new User
            {
                FirstName = "super",
                LastName = "admin",
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "superadmin@alafein.com",
                NormalizedEmail = "SUPERADMIN@ALAFEIN.COM",
                EmailConfirmed = true,
                PhoneNumber = "+201000000000",
                PhoneNumberConfirmed = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedBy = "Alafein System",
                CreatedAt = new DateTime(2023, 11, 8, 0, 0, 0),
                Status = Status.Premium,
                IsBlocked = false,
                IsBlockedFromComment = false,
            };

            await AddSeedAdmin(defaultUser);
        }

        private async Task AddSeedAdmin(User admin)
        {
            await _userManager.CreateAsync(admin);
            await _userManager.AddToRoleAsync(admin, "Super Admin");
        }
    }
}
