using Asp.Versioning;
using Core.DTOs.Event.Response;
using Core.DTOs.Shared;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using DTOs.Shared;
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
    public class AdminEventOrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public AdminEventOrganizerController(IOrganizerService organizerService,
                                             IUploadImageService uploadImageService,
                                             IOptions<FileSettings> fileSettings)
        {
            _organizerService = organizerService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Gets Paginated Event Organizers
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Event Organizers List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<IList<ListEventOrganizerDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _organizerService.GetPagination(new EventOrganizerParameters
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
        /// Get Filter Event Organizers
        /// </summary>
        /// <param name="requestParameters">Pagination params</param>
        /// <response code="200">Get successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [ProducesResponseType(typeof(PagedResponse<IList<ListEventOrganizerDto>>), 200)]
        [HttpPost("GetFilterPaginated")]
        public async Task<IActionResult> GetFilterPaginated([FromBody] EventOrganizerParameters requestParameters)
        {
            var response = await _organizerService.GetPagination(requestParameters);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Add a new Organizer 
        /// </summary>
        /// <param name="model">an object holds the Organizer object</param>
        /// <response code="200">Organizer Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterOrganizerDto model)
        {
            var data = await _organizerService.Register(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
        }

        /// <summary>
        /// Gets Register Organizer Dropdowns
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for register Organizer</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<IList<DropdownViewModel>>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _organizerService.Dropdown();
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
        /// Update Organizer 
        /// </summary>
        /// <param name="model">an object that has The Organizer Details</param>
        /// <response code="200">Organizer Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] EditOrganizerDto model)
        {
            var data = await _organizerService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Gets Event Organizer Details
        /// </summary>
        /// <param name="id">an object holds the id for Event Organizer not user id (not the GUID)</param>
        /// <response code="200">Returns the Event Organizer Details</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Detail/{id}")]
        [ProducesResponseType(typeof(Response<OrganizerDetailDto>), 200)]
        public async Task<IActionResult> Detail([FromRoute] long id)
        {
            var response = await _organizerService.Detail(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Deletes Organizer
        /// </summary>
        /// <param name="id">an object holds the Id of Organizer</param>
        /// <response code="200">Deletes Organizer successfully</response>
        /// <response code="400">Organizer not found, or there is error while saving</response>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var response = await _organizerService.Delete(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
