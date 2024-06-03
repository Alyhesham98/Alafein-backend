using Asp.Versioning;
using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.Interfaces.Event.Services;
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
    public class AdminSubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public AdminSubmissionController(ISubmissionService submissionService,
                                         IUploadImageService uploadImageService,
                                         IOptions<FileSettings> fileSettings)
        {
            _submissionService = submissionService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Upload Poster Image
        /// </summary>
        /// <response code="200">Poster Image Uploaded</response>
        /// <response code="400">Poster Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadPosterImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadPosterImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.PosterPath, "/Poster");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Add A New Submission
        /// </summary>
        /// <param name="model">an object that has Submission Name</param>
        /// <response code="200">Submission Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(Response<long>), 200)]
        public async Task<IActionResult> Add([FromBody] AddAdminSubmissionDto model)
        {
            var data = await _submissionService.AdminAdd(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Gets submission Dropdown
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for submission</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<SubmissionDropdownDto>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _submissionService.Dropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Update Submission 
        /// </summary>
        /// <param name="model">and object that has The Submission Details</param>
        /// <response code="200">Submission Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] UpdateAdminSubmissionDto model)
        {
            var data = await _submissionService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Get Event Details
        /// </summary>
        /// <param name="id">an object holds the Id of Event</param>
        /// <response code="200">Returns the Event Details</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Details/{id}")]
        [ProducesResponseType(typeof(Response<SubmissionDetailDto>), 200)]
        public async Task<IActionResult> Details([FromRoute] long id)
        {
            var response = await _submissionService.Detail(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
