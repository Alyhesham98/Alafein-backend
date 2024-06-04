using Asp.Versioning;
using Core.DTOs.Event.Response;
using Core.Interfaces.Identity.Services;
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
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;
        public OrganizerController(IOrganizerService organizerService)
        {
            _organizerService = organizerService;
        }

        /// <summary>
        /// Gets Event Organizer Details
        /// </summary>
        /// <param name="id">an object holds the id for Event Organizer not user id (not the GUID)</param>
        /// <response code="200">Returns the Event Organizer Details</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Detail/{id}")]
        [AllowAnonymous]
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
    }
}
