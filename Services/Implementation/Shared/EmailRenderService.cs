using Core.Interfaces.Shared.Services;
using DotLiquid;

namespace Services.Implementation.Shared
{
    internal sealed class EmailRenderService : IEmailRenderService
    {
        public EmailRenderService()
        {

        }

        public string RenderLiquidTemplate<T>(string EmailFile, T model)
        {
            // Load the template from a file
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resources/EmailTemplates/", EmailFile);
            string template = File.ReadAllText(templatePath);

            // Create a Hash object with dynamic data
            var data = Hash.FromAnonymousObject(model);

            // Render the template with data and return email body
            return Template.Parse(template).Render(data);
        }
    }
}
