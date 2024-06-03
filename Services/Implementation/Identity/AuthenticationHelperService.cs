using Core.Interfaces.Identity.Services;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Implementation.Identity
{
    public class AuthenticationHelperService : IAuthenticationHelperService

    {

        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly MailSettings _mailSettings;
        public AuthenticationHelperService(IOptions<JWTSettings> jwtSettings,
                                           IEmailService emailService,
                                           IOptions<MailSettings> mailSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
            _mailSettings = mailSettings.Value;
        }


        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public string GeneratePassword(int length)
        {

            bool nonAlphanumeric = true;
            bool digit = true;
            bool lowercase = true;
            bool uppercase = true;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

        public string GetOTP(int length)
        {
            const string chars = "0123456789";

            StringBuilder password = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                password.Append(chars[index]);
            }

            return password.ToString();
        }

        public bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$");
            Match match = regex.Match(password);
            return match.Success;
        }

        public void CreatePasswordHash(string password, out string passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public bool VerifyPasswordHash(string password, string passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(
                    Enumerable.Range(0, passwordHash.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(passwordHash.Substring(x, 2), 16))
                     .ToArray()
                    );
            }
        }

        public async Task<string> generateJwtToken(string employeeId, string role, string securityStamp)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("uid", employeeId),
                new Claim("ss", securityStamp),
                new Claim("ip", GetIpAddress()),
                new Claim(ClaimTypes.Role, role)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        /*public async Task<Response<bool>> SendVerificationMail(string name, string email, string id, string token)
        {
            var mailBody = _emailService.RenderLiquidTemplate("VerifyAccountEmail.html",
                new VerifyEmailRenderModel
                {
                    Id = id,
                    Name = name,
                    Token = token,
                    RepLogo = _mailSettings.Logo,
                    VerifyPath = _mailSettings.VerifyAccountPath
                });

            var verificationMail = await _emailService.SendEmail(new EmailModel
            {
                Body = mailBody,
                Subject = "RepTrust Verification",
                To = email
            });

            if (!verificationMail.Succeeded)
            {
                return new Response<bool>("Failed to resend Verification Mail.");
            }

            return new Response<bool>(true);
        }*/
        /*
        public async Task<Response<bool>> SendForgetPasswordMail(string name, string otp, string email)
        {
            var mailBody = _emailService.RenderLiquidTemplate("ForgetPasswordEmail.html", new ForgetPasswordEmailRenderModel
            {
                Email = email,
                Name = name,
                Otp = otp,
                ForgetPath = _mailSettings.ForgetPath,
                RepLogo = _mailSettings.Logo
            });

            var verificationMail = await _emailService.SendEmail(new EmailModel
            {
                Body = mailBody,
                Subject = "RepTrust Reset Password",
                To = email
            });

            if (!verificationMail.Succeeded)
            {
                return new Response<bool>("Failed to resend Verification Mail.");
            }

            return new Response<bool>(true);
        }*/
    }
}