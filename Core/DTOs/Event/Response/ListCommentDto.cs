using Core.DTOs.User.Response;
using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class ListCommentDto
    {
        public long Id { get; set; }
        public IdentityDropdownDto User { get; set; } = new IdentityDropdownDto();
        public string? ProfilePicture { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DropdownViewModel Event { get; set; } = new DropdownViewModel();
        public string Poster { get; set; } = string.Empty;
    }
}
