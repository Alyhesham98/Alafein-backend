using Core.DTOs.Shared.Email;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Shared.Services
{
    public interface IEmailService
    {
        Task<Response<bool>> SendEmail(EmailModel request);
        Task<Response<bool>> SendEmail(EmailWithAttatchmentsModel request);
        Task<Response<bool>> SendGmailEmail(EmailModel request);
    }
}
