using Asp.Versioning;
using Core.DTOs.Role;
using Core.DTOs.Shared;
using Core.DTOs.User;
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
    [Authorize(Roles = "Super Admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public AdminController(IUserService userService,
                               IUploadImageService uploadImageService,
                               IOptions<FileSettings> fileSettings)
        {
            _userService = userService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Gets Register Dropdown
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for register admin or super admin</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<IList<RoleViewModel>>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _userService.AdminDropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Add a new admin 
        /// </summary>
        /// <param name="model">an object holds the admin object</param>
        /// <response code="200">Admin Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Register([FromBody] UserAddModel model)
        {
            var data = await _userService.RegisterAdmin(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
        }

        /// <summary>
        /// Upload User Image
        /// </summary>
        /// <response code="200">User Image Uploaded</response>
        /// <response code="400">User Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadAdminImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadAdminImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.UserImagesPath, "/User");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Paginated Admins
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Admins List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListAdminDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _userService.GetAdminPagination(new ListAdminParameters
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
        /// Gets Paginated Audience
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Admins List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetAudiencePaginations")]
        [ProducesResponseType(typeof(PagedResponse<List<ListAdminDto>>), 200)]
        public async Task<IActionResult> GetAudiencePaginations([FromQuery] PaginationParameter filter)
        {
            var response = await _userService.GetAudiencePagination(new ListAdminParameters
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
        /// Gets Filter Paginated Audince
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Filtered Admins List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpPost("GetFilterPaginationAudince")]
        [ProducesResponseType(typeof(PagedResponse<List<ListAdminDto>>), 200)]
        public async Task<IActionResult> GetFilterPaginationAudince([FromBody] ListAdminParameters filter)
        {
            var response = await _userService.GetAudiencePagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Filter Paginated Admins
        /// </summary>
        /// <remarks>
        /// Specify either "Admin", "Super Admin", or leave it null to retrieve admins without filtering by role for roleFilter property.
        /// </remarks>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Filtered Admins List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpPost("GetFilterPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListAdminDto>>), 200)]
        public async Task<IActionResult> GetFilterPagination([FromBody] ListAdminParameters filter)
        {
            var response = await _userService.GetAdminPagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets List Admin Dropdown for filter
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for filter admins</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("FilterDropdown")]
        [ProducesResponseType(typeof(Response<List<DropdownViewModel>>), 200)]
        public async Task<IActionResult> FilterDropdown()
        {
            var response = await _userService.ListAdminDropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Update Admin data
        /// </summary>
        /// <param name="model">and object that has Id, Name, Phone and Photo Of The Admin</param>
        /// <response code="200">Admin Updated successfully</response>
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
