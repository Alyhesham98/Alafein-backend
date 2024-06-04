using Asp.Versioning;
using Core.DTOs.User;
using Core.Interfaces.Identity.Services;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Alafein.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class AdminAccountController : ControllerBase
    {
        private readonly IAuthenticationService _accountService;
        public AdminAccountController(IAuthenticationService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="model">an object holds the login object</param>
        /// <response code="200">Employee Login successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(Response<HomeScreenModel>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var data = await _accountService.Login(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
        //    return Forbid();
        }

        /// <summary>
        /// Refresh Token 
        /// </summary>
        /// <param name="request">an object holds the token object</param>
        /// <response code="200">Employee Token Refreshed successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("RefreshToken")]
        [ProducesResponseType(typeof(Response<HomeScreenModel>), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel request)
        {
            var data = await _accountService.RefreshToken(request);
            if (data.Succeeded == false)
            {
                return Unauthorized(data);
            }
            return Ok(data);
           // return Forbid();
        }
    }
}
