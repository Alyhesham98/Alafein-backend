namespace Core.DTOs.User.Response
{
    public class UserCommentDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Photo { get; set; }
    }
}
