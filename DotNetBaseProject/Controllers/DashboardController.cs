using Asp.Versioning;
using Core.DTOs.Dashboard.Request;
using Core.DTOs.Dashboard.Response;
using Core.Interfaces.Dashboard.Services;
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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Gets Dashboard
        /// </summary>
        /// <response code="200">Returns the Dashboard</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dashboard")]
        [ProducesResponseType(typeof(Response<DashboardDto>), 200)]
        public async Task<IActionResult> Dashboard()
        {
            var response = await _dashboardService.Dashboard(new DashboardListParameters());
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Dashboard Dropdown
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for submission</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<List<DropdownViewModel>>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _dashboardService.Dropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Filter Dashboard
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <response code="200">Returns the Filtered Dashboard</response>
        /// <response code="400">something goes wrong in backend</response>
        /// <remarks>
        /// if any of the request body is null then the default filter will be with week
        /// </remarks>
        [HttpPost("GetFilter")]
        [ProducesResponseType(typeof(Response<DashboardDto>), 200)]
        public async Task<IActionResult> GetFilter([FromBody] DashboardListParameters filter)
        {
            var response = await _dashboardService.Dashboard(filter);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
