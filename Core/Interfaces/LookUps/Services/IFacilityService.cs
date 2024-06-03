using Core.DTOs.LookUps.Facility.Request;
using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.LookUps.Services
{
    public interface IFacilityService
    {
        Task<Response<long>> Add(AddFacilityDto request);
        Task<Response<bool>> Update(UpdateFacilityDto request);
        Task<Response<bool>> Delete(long id);
        Task<PagedResponse<IList<ListFacilityDto>>> GetPagination(PaginationParameter filter, bool isAscending = false);
        Task<Response<IList<ListFacilityDto>>> GetWithoutPagination(bool isAscending = false);
    }
}
