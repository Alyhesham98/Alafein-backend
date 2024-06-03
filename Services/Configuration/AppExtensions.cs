using Core.Settings;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Services.Configuration;

namespace MOCA.Services.Configuration
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app,
                                               IConfiguration configuration,
                                               string apiTitle,
                                               bool addCrm,
                                               bool addAdmin,
                                               bool addMobile)
        {
            var swaggerSettings = configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (addCrm)
                {
                    c.SwaggerEndpoint("/swagger/CRM/swagger.json", $"{apiTitle} Content CRM");
                }
                if (addAdmin)
                {
                    c.SwaggerEndpoint("/swagger/Admin/swagger.json", $"{apiTitle} Admin Portal");
                }
                if (addMobile)
                {
                    c.SwaggerEndpoint("/swagger/Mobile/swagger.json", $"{apiTitle} Content Mobile");
                }
                c.RoutePrefix = $"{swaggerSettings!.Key}/swagger";
            });
        }

        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }

        public static void UseStaticFilesExtension(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),

                RequestPath = new PathString("/Resources")

            });
        }

        public static void UseHangfireExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();

            app.UseHangfireDashboard($"/{swaggerSettings!.Key}/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                IgnoreAntiforgeryToken = true
            });
        }
    }
}
