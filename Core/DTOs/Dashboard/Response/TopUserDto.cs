namespace Core.DTOs.Dashboard.Response
{
    public class TopUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public int Count { get; set; }
    }
}
