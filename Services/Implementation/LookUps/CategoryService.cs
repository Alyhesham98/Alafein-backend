using AutoMapper;
using Core;
using Core.DTOs.LookUps.Category.Request;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.Shared;
using Core.Entities.LookUps;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.LookUps.Services;
using DTOs.Shared.Responses;

namespace Services.Implementation.LookUps
{
    internal sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepo,
                               IUnitOfWork unitOfWork,
                               IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<long>> Add(AddCategoryDto request)
        {
            if (await _categoryRepo.Exists(f => f.Name == request.Name))
            {
                return new Response<long>("Category Existed Before.");
            }

            var result = _categoryRepo.Add(_mapper.Map<Category>(request));
            await _unitOfWork.SaveAsync();

            return new Response<long>(result.Id);
        }

        public async Task<Response<bool>> Update(UpdateCategoryDto request)
        {
            var entity = await _categoryRepo.GetByIdAsync(request.Id);
            if (entity is null)
            {
                return new Response<bool>("Category Id not found.");
            }

            if (await _categoryRepo.Exists(f => f.Name == request.Name &&
                                                f.Id != request.Id))
            {
                return new Response<bool>("Category Existed Before.");
            }

            _mapper.Map(request, entity);

            _categoryRepo.Update(entity);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> Delete(long id)
        {
            if (!await _categoryRepo.Exists(f => f.Id == id))
            {
                return new Response<bool>("Category Id not found.");
            }

            var entity = await _categoryRepo.GetByIdAsync(id);

            _categoryRepo.Remove(entity!);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<PagedResponse<IList<ListCategoryDto>>> GetPagination(PaginationParameter filter, bool isAscending = false)
        {
            var pgTotal = await _categoryRepo.GetCountAsync(f => !f.IsDeleted);

            var result = await _categoryRepo.GetPagedWithSelectorAsync(s => new ListCategoryDto
            {
                Id = s.Id,
                Image = s.Image,
                IsPublished = s.IsPublished,
                Name = s.Name,
                SortNo = s.SortNo
            },
            filter.PageNumber,
            filter.PageSize,
            true,
            null,
            isAscending ? o => o.OrderBy(x => x.Id) :
                          o => o.OrderByDescending(x => x.Id));

            return new PagedResponse<IList<ListCategoryDto>>(result, filter.PageNumber, filter.PageSize, pgTotal);
        }

        public async Task<Response<IList<ListCategoryDto>>> GetWithoutPagination(bool isAscending = false)
        {
            var result = await _categoryRepo.GetAllWithSelectorAsync(s => new ListCategoryDto
            {
                Id = s.Id,
                Image = s.Image,
                IsPublished = s.IsPublished,
                Name = s.Name,
                SortNo = s.SortNo
            },
            true,
            null,
            isAscending ? o => o.OrderBy(x => x.Id) :
                          o => o.OrderByDescending(x => x.Id));

            return new Response<IList<ListCategoryDto>>(result);
        }

        public async Task<Response<bool>> TogglePublish(PublishCategoryDto request)
        {
            var category = await _categoryRepo.GetByIdAsync(request.Id);
            if (category is null)
            {
                return new Response<bool>("Category is not Found.");
            }

            if (category.IsPublished)
            {
                category.IsPublished = false;
            }
            else
            {
                category.IsPublished = true;
            }

            _categoryRepo.Update(category);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }
    }
}
