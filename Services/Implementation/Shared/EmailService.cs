using Core.DTOs.Shared.Email;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using DTOs.Shared.Responses;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Services.Implementation.Shared
{
    internal sealed class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<Response<bool>> SendEmail(EmailModel request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_mailSettings.Email);
            mail.To.Add(request.To);
            mail.Subject = request.Subject;
            mail.Body = request.Body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(_mailSettings.Host);

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
            smtp.Port = int.Parse(_mailSettings.Port);
            smtp.EnableSsl = false;
            smtp.Send(mail);

            return new Response<bool>(true, "Email Sent successfully.");
        }

        public async Task<Response<bool>> SendEmail(EmailWithAttatchmentsModel request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_mailSettings.Email);
            mail.To.Add(request.To);
            mail.Subject = request.Subject;
            mail.Body = request.Body;
            mail.IsBodyHtml = true;

            if (request.Files is not null &&
                request.Files.Count > 0)
            {
                foreach (var file in request.Files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            var fileBytes = ms.ToArray();
                            Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                            mail.Attachments.Add(att);
                        }
                    }
                }
            }

            SmtpClient smtp = new SmtpClient(_mailSettings.Host);

            NetworkCredential Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = Credentials;
            smtp.Port = int.Parse(_mailSettings.Port);
            smtp.EnableSsl = false;
            smtp.Send(mail);

            return new Response<bool>(true, "Email Sent successfully.");
        }

        public async Task<Response<bool>> SendGmailEmail(EmailModel request)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName);
            mail.To.Add(request.To);
            mail.Subject = request.Subject;
            mail.Body = request.Body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(_mailSettings.Host, int.Parse(_mailSettings.Port));

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password);
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return new Response<bool>(true, "Email Sent successfully.");
        }
    }
}
