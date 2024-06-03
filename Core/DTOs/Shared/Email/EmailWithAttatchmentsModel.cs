using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Shared.Email
{
    public class EmailWithAttatchmentsModel : EmailModel
    {
        public IList<IFormFile> Files { get; set; }
    }
}
