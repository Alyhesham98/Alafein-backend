using Microsoft.AspNetCore.Http;

namespace DTOs.Shared
{
    public class FileUpload
    {
        public string FolderName { get; set; } = string.Empty;
        public IFormFile FormFile { get; set; }
    }
}
