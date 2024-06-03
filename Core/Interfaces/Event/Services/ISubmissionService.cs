using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Event.Services
{
    public interface ISubmissionService
    {
        Task<Response<long>> Add(AddSubmissionDto request);
        Task<Response<SubmissionDropdownDto>> Dropdown();
        Task<Response<long>> AdminAdd(AddAdminSubmissionDto request);
        Task<Response<bool>> Update(UpdateAdminSubmissionDto request);
        Task<Response<AddAdminSubmissionDto>> GetDetails(long request);
        Task<Response<SubmissionDetailDto>> Detail(long id);
    }
}
