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
    [Authorize(Roles = "Host Venue,Audience")]
    [ApiExplorerSettings(GroupName = "Mobile")]
    public class NotificationHistoryController : ControllerBase
    {
        private readonly INotificationHistoryService _notificationHistoryService;
        public NotificationHistoryController(INotificationHistoryService notificationHistoryService)
        {
            _notificationHistoryService = notificationHistoryService;
        }

        /// <summary>
        /// Gets Paginated Notifications
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Notifications List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListNotificationHistoryDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _notificationHistoryService.GetPagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Toggle Notification Read Status
        /// </summary>
        /// <param name="model">and object that has Id Of The Notification</param>
        /// <response code="200">Notification Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("ToggleRead")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleRead([FromBody] UpdateNotificationHistoryDto model)
        {
            var data = await _notificationHistoryService.Read(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }
    }
}
