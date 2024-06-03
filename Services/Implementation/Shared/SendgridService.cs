using Core.DTOs.Shared.Email;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using DTOs.Shared.Responses;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Services.Implementation.Shared
{
    internal sealed class SendgridService : ISendgridService
    {
        private readonly SendGridSettings _sendGridSettings;
        public SendgridService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public async Task<Response<bool>> SendEmail(EmailModel request)
        {
            var client = new SendGridClient(_sendGridSettings.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.SenderEmail),
                Subject = request.Subject,
                HtmlContent = request.Body,
                ReplyTo = new EmailAddress(request.To)
            };

            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                return new Response<bool>(true);
            }
            return new Response<bool>("Error Email sent.");
        }

    }
}
