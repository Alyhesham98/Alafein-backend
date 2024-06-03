using Asp.Versioning;
using Core.DTOs.Shared;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Interfaces.Identity.Services;
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
    public class AdminVenueController : ControllerBase
    {
        private readonly IVenueService _venueService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public AdminVenueController(IVenueService venueService,
                                    IUploadImageService uploadImageService,
                                    IOptions<FileSettings> fileSettings)
        {
            _venueService = venueService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Gets Paginated Venues
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Venues List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<IList<ListVenueDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _venueService.GetPagination(new VenueParameters
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            });
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Filter Venues
        /// </summary>
        /// <param name="requestParameters">Pagination params</param>
        /// <response code="200">Get successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [ProducesResponseType(typeof(PagedResponse<IList<ListVenueDto>>), 200)]
        [HttpPost("GetFilterPaginated")]
        public async Task<IActionResult> GetFilterPaginated([FromBody] VenueParameters requestParameters)
        {
            var response = await _venueService.GetPagination(requestParameters);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Add a new venue 
        /// </summary>
        /// <param name="model">an object holds the venue object</param>
        /// <response code="200">Venue Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterVenueDto model)
        {
            var data = await _venueService.Register(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
        }

        /// <summary>
        /// Gets Register Venue Dropdowns
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for register venue</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<RegisterVenueDropdownDto>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _venueService.Dropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Upload User Image
        /// </summary>
        /// <response code="200">User Image Uploaded</response>
        /// <response code="400">User Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadUserImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadUserImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.UserImagesPath, "/User");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Upload Venue Image
        /// </summary>
        /// <response code="200">Venue Image Uploaded</response>
        /// <response code="400">Venue Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadVenueImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadVenueImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.VenuePath, "/Venue");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
