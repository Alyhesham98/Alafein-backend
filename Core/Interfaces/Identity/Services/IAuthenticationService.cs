using Core.DTOs.User;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Identity.Services
{
    public interface IAuthenticationService
    {
        Task<Response<bool>> SendOtp(SendOtpDto request);
        Task<Response<HomeScreenModel>> ValidateOtp(VerifyOtpDto request);
        Task<Response<string>> Register(RegisterDto request);
        Task<Response<HomeScreenModel>> Login(LoginModel request);
        Task<Response<HomeScreenModel>> MobileLogin(MobileLoginModel request);
        Task<Response<HomeScreenModel>> RefreshToken(RefreshTokenModel request);
        Task<Response<HomeScreenModel>> GoogleAuth(ExternalLoginViewModel request);
        Task<Response<HomeScreenModel>> FacebookAuth(ExternalLoginViewModel request);
        Task<Response<RegisterDropdownDto>> Dropdown();
    }
}
