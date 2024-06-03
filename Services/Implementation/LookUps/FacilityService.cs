using AutoMapper;
using Core;
using Core.DTOs.LookUps.Facility.Request;
using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.Shared;
using Core.Entities.LookUps;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.LookUps.Repositories;
using Core.Interfaces.LookUps.Services;
using DTOs.Shared.Responses;

namespace Services.Implementation.LookUps
{
    internal sealed class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepo;
        private readonly IVenueFacilityRepository _venueFacilityRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FacilityService(IFacilityRepository facilityRepo,
                               IVenueFacilityRepository venueFacilityRepo,
                               IUnitOfWork unitOfWork,
                               IMapper mapper)
        {
            _facilityRepo = facilityRepo;
            _venueFacilityRepo = venueFacilityRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<long>> Add(AddFacilityDto request)
        {
            var result = _facilityRepo.Add(_mapper.Map<Facility>(request));
            await _unitOfWork.SaveAsync();

            return new Response<long>(result.Id);
        }

        public async Task<Response<bool>> Update(UpdateFacilityDto request)
        {
            var entity = await _facilityRepo.GetByIdAsync(request.Id);
            if (entity is null)
            {
                return new Response<bool>("Facility Id not found.");
            }

            _mapper.Map(request, entity);

            _facilityRepo.Update(entity);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<Response<bool>> Delete(long id)
        {
            if (!await _facilityRepo.Exists(f => f.Id == id))
            {
                return new Response<bool>("Facility Id not found.");
            }

            if (!await _venueFacilityRepo.Exists(f => f.FacilityId == id))
            {
                return new Response<bool>("Facility Id already in use.");
            }

            var entity = await _facilityRepo.GetByIdAsync(id);

            _facilityRepo.Remove(entity!);
            await _unitOfWork.SaveAsync();

            return new Response<bool>(true);
        }

        public async Task<PagedResponse<IList<ListFacilityDto>>> GetPagination(PaginationParameter filter, bool isAscending = false)
        {
            var pgTotal = await _facilityRepo.GetCountAsync(f => !f.IsDeleted);

            var result = await _facilityRepo.GetPagedWithSelectorAsync(s => new ListFacilityDto
            {
                Id = s.Id,
                ImageName = s.ImageName,
                ImagePath = s.ImagePath
            },
            filter.PageNumber,
            filter.PageSize,
            true,
            null,
            isAscending ? o => o.OrderBy(x => x.Id) :
                          o => o.OrderByDescending(x => x.Id));

            return new PagedResponse<IList<ListFacilityDto>>(result, filter.PageNumber, filter.PageSize, pgTotal);
        }

        public async Task<Response<IList<ListFacilityDto>>> GetWithoutPagination(bool isAscending = false)
        {
            var result = await _facilityRepo.GetAllWithSelectorAsync(s => new ListFacilityDto
            {
                Id = s.Id,
                ImageName = s.ImageName,
                ImagePath = s.ImagePath
            },
            true,
            null,
            isAscending ? o => o.OrderBy(x => x.Id) :
                          o => o.OrderByDescending(x => x.Id));

            return new Response<IList<ListFacilityDto>>(result);
        }
    }
}
