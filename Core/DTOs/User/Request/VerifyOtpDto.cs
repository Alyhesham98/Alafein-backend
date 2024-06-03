namespace Core.DTOs.User.Request
{
    public class VerifyOtpDto : SendOtpDto
    {
        public string OTP { get; set; } = string.Empty;
    }
}
