using Asp.Versioning;
using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.DTOs.Shared;
using Core.Interfaces.Event.Services;
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
    public class AdminCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public AdminCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Toggle Submission Comment
        /// </summary>
        /// <param name="model">and object that has Id Of The Submission Comment</param>
        /// <response code="200">Submission Comment Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("ToggleComment")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> ToggleComment([FromBody] ToggleCommentDto model)
        {
            var data = await _commentService.ToggleComment(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Gets Paginated Comments
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Comments List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListCommentDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter)
        {
            var response = await _commentService.GetPagination(new CommentListParameters
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
        /// Gets Filter Paginated Comments
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Filtered Comments List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpPost("GetFilterPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListCommentDto>>), 200)]
        public async Task<IActionResult> GetFilterPagination([FromBody] CommentListParameters filter)
        {
            var response = await _commentService.GetPagination(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
