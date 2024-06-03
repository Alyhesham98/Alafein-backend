using Asp.Versioning;
using Core.DTOs.User.Response;
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
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;
        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /// <summary>
        /// Gets Venue Details
        /// </summary>
        /// <param name="id">an object holds the id for venue not user id (not the GUID)</param>
        /// <response code="200">Returns the Venue Details</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Detail/{id}")]
        [ProducesResponseType(typeof(Response<VenueDetailDto>), 200)]
        public async Task<IActionResult> Detail([FromRoute] long id)
        {
            var response = await _venueService.Detail(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
