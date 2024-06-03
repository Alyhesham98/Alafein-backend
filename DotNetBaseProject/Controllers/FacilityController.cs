using Asp.Versioning;
using Core.DTOs.LookUps.Facility.Request;
using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.Shared;
using Core.Interfaces.LookUps.Services;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Alafein.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Super Admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _facilityService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public FacilityController(IFacilityService facilityService,
                                  IUploadImageService uploadImageService,
                                  IOptions<FileSettings> fileSettings)
        {
            _facilityService = facilityService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Add A New Facility
        /// </summary>
        /// <param name="model">an object that has Facility Name</param>
        /// <response code="200">Facility Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(Response<long>), 200)]
        public async Task<IActionResult> Add([FromBody] AddFacilityDto model)
        {
            var data = await _facilityService.Add(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Update Facility 
        /// </summary>
        /// <param name="model">and object that has Id, and Name Of The Facility</param>
        /// <response code="200">Facility Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] UpdateFacilityDto model)
        {
            var data = await _facilityService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Deletes Facility
        /// </summary>
        /// <param name="id">an object holds the Id of Facility</param>
        /// <response code="200">Deletes Facility successfully</response>
        /// <response code="400">Facility not found, or there is error while saving</response>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var response = await _facilityService.Delete(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Paginated Facilitys
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <param name="isAscending">indicator for ordering data</param>
        /// <response code="200">Returns the Facilitys List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListFacilityDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter,
                                                       bool isAscending = false)
        {
            var response = await _facilityService.GetPagination(filter, isAscending);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Facilitys Without Pagination
        /// </summary>
        /// <param name="isAscending">indicator for ordering data</param>
        /// <response code="200">Returns the Facilitys List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetWithoutPagination")]
        [ProducesResponseType(typeof(Response<List<ListFacilityDto>>), 200)]
        public async Task<IActionResult> GetWithoutPagination(bool isAscending = false)
        {
            var response = await _facilityService.GetWithoutPagination(isAscending);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Upload Facility Image
        /// </summary>
        /// <response code="200">Facility Image Uploaded</response>
        /// <response code="400">Facility Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadFacilityImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadFacilityImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.FacilityPath, "/Facility");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
