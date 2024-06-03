using Asp.Versioning;
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
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public AdminUserController(IUserService userService,
                                   IUploadImageService uploadImageService,
                                   IOptions<FileSettings> fileSettings)
        {
            _userService = userService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Gets Paginated Users
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Users List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<UserDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _userService.GetPagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Toggle User Block Status
        /// </summary>
        /// <param name="model">and object that has Id Of The User</param>
        /// <response code="200">User Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("ToggleBlock")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleBlock([FromBody] ToggleBlockDto model)
        {
            var data = await _userService.ToggleBlock(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Toggle User Status
        /// </summary>
        /// <param name="model">and object that has Id, and Status Of The User</param>
        /// <response code="200">User Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("ToggleStatus")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleStatus([FromBody] ToggleStatusDto model)
        {
            var data = await _userService.ToggleStatus(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Gets edit user Dropdown
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for edit user</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<List<DropdownViewModel>>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _userService.Dropdown();
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
        /// Update User data
        /// </summary>
        /// <param name="model">and object that has Id, Name, Phone and Photo Of The User</param>
        /// <response code="200">User Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("Update")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto model)
        {
            var data = await _userService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }
    }
}
