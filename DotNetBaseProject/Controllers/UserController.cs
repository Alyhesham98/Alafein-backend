using Asp.Versioning;
using Core.DTOs.User;
using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Alafein.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "Mobile")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _accountService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public UserController(IUserService userService,
                              IAuthenticationService accountService,
                              IUploadImageService uploadImageService,
                              IOptions<FileSettings> fileSettings)
        {
            _userService = userService;
            _accountService = accountService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Add a new client 
        /// </summary>
        /// <param name="model">an object holds the client object</param>
        /// <response code="200">Client Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var data = await _accountService.Register(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
           // return Forbid();
        }

        /// <summary>
        /// Login 
        /// </summary>
        /// <param name="model">an object holds the login object</param>
        /// <response code="200">User Login successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(Response<HomeScreenModel>), 200)]
        public async Task<IActionResult> Login([FromBody] MobileLoginModel model)
        {
            var data = await _accountService.MobileLogin(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
           // return Forbid();
        }

        /// <summary>
        /// send otp for user to login with 
        /// </summary>
        /// <param name="model">an object holds the email of user</param>
        /// <remarks>
        /// User can send up to 3 email per day if he try to send more otps a validaton message would appear.
        /// </remarks>
        /// <response code="200">user otp email sent successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("SendOtp")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDto model)
        {
            var data = await _accountService.SendOtp(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
        }

        /// <summary>
        /// Validate otop entred for user to check if that's the latest one. 
        /// </summary>
        /// <param name="model">an object holds the validate otp object</param>
        /// <response code="200">User Login successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("ValidateOtp")]
        [ProducesResponseType(typeof(Response<HomeScreenModel>), 200)]
        public async Task<IActionResult> ValidateOtp([FromBody] VerifyOtpDto model)
        {
            var data = await _accountService.ValidateOtp(model);

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
        [HttpPost("UploadUserImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadUserImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.UserImagesPath, "/User");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Upload Venue Image
        /// </summary>
        /// <response code="200">Venue Image Uploaded</response>
        /// <response code="400">Venue Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadVenueImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadVenueImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.VenuePath, "/Venue");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
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

        /// <summary>
        /// Gets Register Dropdowns
        /// </summary>
        /// <response code="200">Returns the dropdown Lists required for register</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Dropdown")]
        [ProducesResponseType(typeof(Response<RegisterDropdownDto>), 200)]
        public async Task<IActionResult> Dropdown()
        {
            var response = await _accountService.Dropdown();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Update User data
        /// </summary>
        /// <param name="model">and object that has Id, Name, Phone and Photo Of The User</param>
        /// <response code="200">User Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("Update")]
        [Authorize(Roles = "Audience,Host Venue")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] UserProfileDto model)
        {
            var data = await _userService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Gets User Profile Data
        /// </summary>
        /// <response code="200">Returns the User Profile Data</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("Profile")]
        [ProducesResponseType(typeof(Response<MobileUserDetailDto>), 200)]
        public async Task<IActionResult> Profile()
        {
            var response = await _userService.Profile();
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Google Auth 
        /// </summary>
        /// <param name="model">an object holds the login token</param>
        /// <response code="200">User Auth successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("GoogleAuth")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> GoogleAuth([FromBody] ExternalLoginViewModel model)
        {
            var data = await _accountService.GoogleAuth(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
            // return Forbid();
        }

        /// <summary>
        /// Facebook Auth 
        /// </summary>
        /// <param name="model">an object holds the login token</param>
        /// <response code="200">User Auth successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("FacebookAuth")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> FacebookAuth([FromBody] ExternalLoginViewModel model)
        {
            var data = await _accountService.FacebookAuth(model);

            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }

            return Ok(data);
        //    return Forbid();
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="userId">The id of the user to be deleted</param>
        /// <response code="200">User Deleted successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpDelete("Delete/{userId}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Delete(string userId)
        {
            var data = await _userService.Delete(userId);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }
    }
}
