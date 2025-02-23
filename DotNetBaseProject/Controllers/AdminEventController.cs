using Asp.Versioning;
using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.Shared;
using Core.Interfaces.Event.Services;
using DTOs.Shared;
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
    public class AdminEventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public AdminEventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Gets Paginated Events
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListEventDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _eventService.GetPagination(new EventListParameters
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
        /// Gets Filter Paginated Events
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Filtered Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpPost("GetFilterPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListEventDto>>), 200)]
        public async Task<IActionResult> GetFilterPagination([FromBody] EventListParameters filter)
        {
            var response = await _eventService.GetPagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Toggle Submission Status
        /// </summary>
        /// <param name="model">and object that has Id, and Status Of The Submission</param>
        /// <response code="200">Submission Status Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("ToggleStatus")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleStatus([FromBody] ToggleSubmissionStatusDto model)
        {
            var data = await _eventService.ToggleStatus(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Gets event submission status Dropdown
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for edit event submission status</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<List<DropdownViewModel>>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _eventService.Dropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Toggle Submission Spotlight
        /// </summary>
        /// <param name="model">and object that has Id Of The Submission</param>
        /// <response code="200">Submission Spotlight Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("ToggleSpotlight")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleSpotlight([FromBody] ToggleSubmissionSpotlightDto model)
        {
            var data = await _eventService.ToggleSpotlight(model);
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
        [ProducesResponseType(typeof(Response<EventDetailDto>), 200)]
        public async Task<IActionResult> Details([FromRoute] long id)
        {
            var response = await _eventService.Detail(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Paginated Spotlight Events
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Spotlight Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetSpotlightPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListEventDto>>), 200)]
        public async Task<IActionResult> GetSpotlightPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _eventService.GetPaginationSpotLight(new EventListParameters
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                IsSpotlight = true
            });
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Paginated Pending Events
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Pending Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPendingPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListEventDto>>), 200)]
        public async Task<IActionResult> GetPendingPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _eventService.GetPagination(new EventListParameters
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                IsPending = true
            });
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Order Spotlight
        /// </summary>
        /// <param name="model">and object that has Id, and Order Of The Event</param>
        /// <response code="200">Event Order Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("SpotlightOrder")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> SpotlightOrder([FromBody] SpotlightOrderDto model)
        {
            var data = await _eventService.SpotlightOrder(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }
    }
}
