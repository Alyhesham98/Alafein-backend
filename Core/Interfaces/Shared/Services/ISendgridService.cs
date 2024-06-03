using Core.DTOs.Shared.Email;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Shared.Services
{
    public interface ISendgridService
    {
        Task<Response<bool>> SendEmail(EmailModel request);
    }
}
