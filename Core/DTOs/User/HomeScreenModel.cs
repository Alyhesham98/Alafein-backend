namespace Core.DTOs.User
{
    public class HomeScreenModel
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
        public DateTime TokenExpiration { get; set; }
        public string JWTToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }

    }
}
