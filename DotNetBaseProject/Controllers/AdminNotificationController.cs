using Asp.Versioning;
using Core.DTOs.Alert.Request;
using Core.DTOs.Alert.Response;
using Core.DTOs.Shared;
using Core.Interfaces.Alert.Services;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alafein.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Super Admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class AdminNotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public AdminNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Add A New Notification
        /// </summary>
        /// <param name="model">an object that has Notification Name</param>
        /// <response code="200">Notification Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(Response<long>), 200)]
        public async Task<IActionResult> Add([FromBody] AddNotificationDto model)
        {
            var data = await _notificationService.Add(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Update Notification 
        /// </summary>
        /// <param name="model">and object that has Id, and Name Of The Notification</param>
        /// <response code="200">Notification Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] UpdateNotificationDto model)
        {
            var data = await _notificationService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Deletes Notification
        /// </summary>
        /// <param name="id">an object holds the Id of Notification</param>
        /// <response code="200">Deletes Notification successfully</response>
        /// <response code="400">Notification not found, or there is error while saving</response>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var response = await _notificationService.Delete(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Paginated Notifications
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <param name="isAscending">indicator for ordering data</param>
        /// <response code="200">Returns the Notifications List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListNotificationDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter,
                                                       bool isAscending = false)
        {
            var response = await _notificationService.GetPagination(filter, isAscending);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
