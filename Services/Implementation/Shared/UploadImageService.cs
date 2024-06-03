using Core.Interfaces.Shared.Services;
using DTOs.Shared.Responses;
using Microsoft.AspNetCore.Http;

namespace MOCA.Services.Implementation.Shared
{
    public class UploadImageService : IUploadImageService
    {
        public UploadImageService()
        {

        }

        public async Task<Response<string>> UploadImage(IFormFile image, string fileSetting, string folder)
        {
            if (image == null || image.Length == 0)
            {
                return new Response<string>("No file uploaded.");
            }

            if (!IsImage(image.ContentType))
            {
                return new Response<string>("File Uploaded is not an Image");
            }

            var fileExtension = Path.GetExtension(image.FileName);
            if (string.IsNullOrEmpty(fileExtension))
            {
                return new Response<string>("Unable to determine file extension.");
            }

            var imageName = $"{Guid.NewGuid()}{fileExtension}";

            string? folderName = fileSetting + folder;

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, imageName);
            var dbPath = Path.Combine(folderName, imageName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return new Response<string>(dbPath, "Image Uploaded Successfully");
        }

        private bool IsImage(string contentType)
        {
            return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<Response<bool>> RemoveFromCurrentDirectory(string Image)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), Image);
            File.Delete(imagePath);
            return new Response<bool>(true, "Images Deleted successfully");
        }
    }
}
