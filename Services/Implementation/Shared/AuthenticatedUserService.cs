using Core.Interfaces.Shared.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Services.Implementation.Shared
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            var User_Id = httpContextAccessor.HttpContext?.User?.FindFirst("uid");
            if (User_Id is not null)
            {
                UserId = User_Id.Value;
            }

            var roleClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);
            if (roleClaim is not null)
            {
                Role = roleClaim.Value;
            }
        }

        public string? UserId { get; }
        public string? Role { get; }
    }
}
