using Core.DTOs.Shared;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Shared.Services
{
    public interface IUploadImageService
    {
        Task<Response<string>> UploadImage(IFormFile image, string fileSetting, string folder);
        Task<Response<bool>> RemoveFromCurrentDirectory(string Image);
    }
}
