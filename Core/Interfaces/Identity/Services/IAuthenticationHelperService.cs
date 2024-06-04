using DTOs.Shared.Responses;

namespace Core.Interfaces.Identity.Services
{
    public interface IAuthenticationHelperService
    {
        string CreateRandomToken();
        string GeneratePassword(int length);
        string GetOTP(int length);
        bool ValidatePassword(string password);
        void CreatePasswordHash(string password, out string passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, string passwordHash, byte[] passwordSalt);
        Task<string> generateJwtToken(string employeeId, string role, string securityStamp);
        string GetIpAddress();
        Task<Response<bool>> SendVerificationMail(string name, string email, string id, string token);
        Task<Response<bool>> SendForgetPasswordMail(string name, string otp, string email);
    }
}
