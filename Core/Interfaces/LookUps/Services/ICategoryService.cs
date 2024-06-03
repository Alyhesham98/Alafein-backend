using Core.DTOs.LookUps.Category.Request;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.LookUps.Services
{
    public interface ICategoryService
    {
        Task<Response<long>> Add(AddCategoryDto request);
        Task<Response<bool>> Update(UpdateCategoryDto request);
        Task<Response<bool>> Delete(long id);
        Task<PagedResponse<IList<ListCategoryDto>>> GetPagination(PaginationParameter filter, bool isAscending = false);
        Task<Response<IList<ListCategoryDto>>> GetWithoutPagination(bool isAscending = false);
        Task<Response<bool>> TogglePublish(PublishCategoryDto request);
    }
}
