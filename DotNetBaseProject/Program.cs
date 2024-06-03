using Asp.Versioning;
using Core.Interfaces.Identity.Services;
using MOCA.Services.Configuration;
using Presistence.Configuration;
using Services.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddEndpointsSetup()
                .AddSwagger(configuration: builder.Configuration,
                            apiTitle: "ALAFEIN.API",
                            apiVersion: "v1",
                            xmlPath,
                            addSecurity: true,
                            addCrm: false,
                            addAdmin: true,
                            addMobile: true)
                .AddApiVersioningSetup(defaultApiVersion: new ApiVersion(1, 0))
                .AddIdentitySetup()
                .AddJWTAuthentication(configuration: builder.Configuration)
                .AddPersistence(configuration: builder.Configuration, connectionStringName: "DefaultConnection")
                .AddCoreServices(configuration: builder.Configuration)
                .AddMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddEmailConfiguration(configuration: builder.Configuration)
                .ConfigureSendGridSettings(configuration: builder.Configuration)
                .ConfigureFirebaseSettings(configuration: builder.Configuration)
                .AddNativeRateLimiting(configuration: builder.Configuration)
                .AddHangfireExtension(configuration: builder.Configuration)
                .AddIdentityServices()
                .AddLookUpServices()
                .AddCRMServices()
                .AddMobileServices()
                ;

var app = builder.Build();

app.UseErrorHandlingMiddleware();

app.UseSwaggerExtension(configuration: builder.Configuration, "ALAFEIN.API v1", addCrm: false, addAdmin: true, addMobile: true);

using (var scope = app.Services.CreateScope())
{
    var seedAdminService = scope.ServiceProvider.GetRequiredService<ISeedSuperAdminDataService>();
    await seedAdminService.SeedAdminAsync();
}

app.UseHttpsRedirection();

app.UseStaticFilesExtension();

app.UseRouting();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.UseHangfireExtension(configuration: builder.Configuration);

app.MapControllers();

app.Run();