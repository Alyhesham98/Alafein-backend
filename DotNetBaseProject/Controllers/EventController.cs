using Asp.Versioning;
using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.Shared;
using Core.Interfaces.Event.Services;
using Core.Interfaces.LookUps.Services;
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
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;
        public EventController(IEventService eventService,
                               ICommentService commentService,
                               ICategoryService categoryService)
        {
            _eventService = eventService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets Paginated Events
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResponse<List<ListEventMobileDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            /*var response = await _eventService.GetMobilePagination(new EventMobileListParameters
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            });
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);*/
            return Forbid();
        }

        /// <summary>
        /// Gets Filter Paginated Events
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Filtered Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpPost("GetFilterPagination")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResponse<List<ListEventMobileDto>>), 200)]
        public async Task<IActionResult> GetFilterPagination([FromBody] EventMobileListParameters filter)
        {
            /*var response = await _eventService.GetMobilePagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);*/
            return Forbid();
        }

        /// <summary>
        /// Get Event Details
        /// </summary>
        /// <param name="id">an object holds the Id of Event</param>
        /// <response code="200">Returns the Event Details</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Details/{id}")]
        [AllowAnonymous]
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
        /// Gets Home Event
        /// </summary>
        /// <response code="200">Returns the Home Events List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Home")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<HomeDto>), 200)]
        public async Task<IActionResult> Home()
        {
            /*var response = await _eventService.Home();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);*/
            return Forbid();
        }

        /// <summary>
        /// Toggle Favourite Submission for user
        /// </summary>
        /// <param name="model">an object that has Id of the submission</param>
        /// <response code="200">Submission (Added Or Removed) successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("ToggleFavourite")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleFavourite([FromBody] ToggleSubmissionFavouriteDto model)
        {
            var data = await _eventService.ToggleFavourite(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Add comment for Submission
        /// </summary>
        /// <param name="model">an object that has Id, and comment for the submission</param>
        /// <response code="200">Comment added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Comment")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Comment([FromBody] CommentDto model)
        {
            var data = await _eventService.Comment(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Get Event Approved Comments
        /// </summary>
        /// <param name="id">an object holds the Id of Event</param>
        /// <response code="200">Returns the Event Approved Comments List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Comments/{id}")]
        [ProducesResponseType(typeof(Response<IList<EventCommentDto>>), 200)]
        public async Task<IActionResult> Comments([FromRoute] long id)
        {
            var response = await _commentService.GetEventComments(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Categorys Without Pagination
        /// </summary>
        /// <param name="isAscending">indicator for ordering data</param>
        /// <response code="200">Returns the Categorys List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetCategories")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<List<ListCategoryDto>>), 200)]
        public async Task<IActionResult> GetCategories(bool isAscending = false)
        {
            /*var response = await _categoryService.GetWithoutPagination(isAscending);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);*/
            return Forbid();
        }

        /// <summary>
        /// Gets Minmum and Maximum value for fee to filter with
        /// </summary>
        /// <response code="200">Returns the fee min and max values</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("FeeConfiguration")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<FeeDto>), 200)]
        public async Task<IActionResult> FeeConfiguration()
        {
            /*var response = await _eventService.FeeConfiguration();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);*/
            return Forbid();
        }
    }
}
