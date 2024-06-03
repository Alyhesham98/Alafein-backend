using Asp.Versioning;
using Core.DTOs.LookUps.Category.Request;
using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.Shared;
using Core.Interfaces.LookUps.Services;
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
    [Authorize(Roles = "Admin,Super Admin")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUploadImageService _uploadImageService;
        private readonly FileSettings _fileSettings;
        public CategoryController(ICategoryService categoryService,
                                  IUploadImageService uploadImageService,
                                  IOptions<FileSettings> fileSettings)
        {
            _categoryService = categoryService;
            _uploadImageService = uploadImageService;
            _fileSettings = fileSettings.Value;
        }

        /// <summary>
        /// Add A New Category
        /// </summary>
        /// <param name="model">an object that has Category Name</param>
        /// <response code="200">Category Added successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(Response<long>), 200)]
        public async Task<IActionResult> Add([FromBody] AddCategoryDto model)
        {
            var data = await _categoryService.Add(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Update Category 
        /// </summary>
        /// <param name="model">an object that has Id, and Name Of The Category</param>
        /// <response code="200">Category Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDto model)
        {
            var data = await _categoryService.Update(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        /// <summary>
        /// Deletes Category
        /// </summary>
        /// <param name="id">an object holds the Id of Category</param>
        /// <response code="200">Deletes Category successfully</response>
        /// <response code="400">Category not found, or there is error while saving</response>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var response = await _categoryService.Delete(id);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Paginated Categorys
        /// </summary>
        /// <param name="filter">an object holds the filter data</param>
        /// <param name="isAscending">indicator for ordering data</param>
        /// <response code="200">Returns the Categorys List</response>
        /// <response code="400">something goes wrong in backend</response>
        [HttpGet("GetPagination")]
        [ProducesResponseType(typeof(PagedResponse<List<ListCategoryDto>>), 200)]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParameter filter,
                                                       bool isAscending = false)
        {
            var response = await _categoryService.GetPagination(filter, isAscending);
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
        [HttpGet("GetWithoutPagination")]
        [ProducesResponseType(typeof(Response<List<ListCategoryDto>>), 200)]
        public async Task<IActionResult> GetWithoutPagination(bool isAscending = false)
        {
            var response = await _categoryService.GetWithoutPagination(isAscending);
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Upload Category Image
        /// </summary>
        /// <response code="200">Category Image Uploaded</response>
        /// <response code="400">Category Image Not Uploaded, or there is error while saving</response>
        [HttpPost("UploadCategoryImage")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UploadCategoryImage(IFormFile image)
        {
            var response = await _uploadImageService.UploadImage(image, _fileSettings.CategoryPath, "/Category");
            if (response.Succeeded == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Toggle Category Status
        /// </summary>
        /// <param name="model">and object that has Id Of The Category</param>
        /// <response code="200">Category Updated successfully</response>
        /// <response code="400">If the request is badly formatted or the data cannot be processed.</response>
        [HttpPatch("TogglePublish")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        public async Task<IActionResult> TogglePublish([FromBody] PublishCategoryDto model)
        {
            var data = await _categoryService.TogglePublish(model);
            if (data.Succeeded == false)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }
    }
}
