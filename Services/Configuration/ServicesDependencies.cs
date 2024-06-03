using Core.Interfaces.Alert.Services;
using Core.Interfaces.Dashboard.Services;
using Core.Interfaces.Event.Services;
using Core.Interfaces.Identity.Services;
using Core.Interfaces.LookUps.Services;
using Core.Interfaces.Shared.Services;
using Core.Settings;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MOCA.Services.Implementation.Shared;
using Services.Implementation.Alert;
using Services.Implementation.Dashboard;
using Services.Implementation.Event;
using Services.Implementation.Identity;
using Services.Implementation.LookUps;
using Services.Implementation.Shared;
using System.Reflection;

namespace Services.Configuration
{
    public static class ServicesDependencies
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services,
                                                         IConfiguration configuration)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped<IPhoneNumberService, PhoneNumberService>();
            services.AddScoped<IUploadImageService, UploadImageService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IEmailRenderService, EmailRenderService>();

            services.Configure<FileSettings>(configuration.GetSection("FileSettings"));

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services,
                                                   Assembly[] assemblies)
        {
            services.AddAutoMapper(assemblies);
            return services;
        }

        /// <summary>
        /// Add Email Configuration
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>        
        /// <param name="configuration">IConfiguration to access jwt settings</param>
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services,
                                                               IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        public static IServiceCollection ConfigureSendGridSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISendgridService, SendgridService>();
            services.Configure<SendGridSettings>(configuration.GetSection("SendGridSettings"));

            return services;
        }

        /// <summary>
        /// Add Identity Services
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>        
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuthenticationHelperService, AuthenticationHelperService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ISeedSuperAdminDataService, SeedSuperAdminDataService>();

            return services;
        }

        /// <summary>
        /// Add LookUp Services
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>        
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddLookUpServices(this IServiceCollection services)
        {
            services.AddTransient<IFacilityService, FacilityService>();
            services.AddTransient<ICategoryService, CategoryService>();

            return services;
        }

        /// <summary>
        /// Add CRM Services
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>        
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddCRMServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVenueService, VenueService>();
            services.AddTransient<IOrganizerService, OrganizerService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<INotificationService, NotificationService>();

            return services;
        }

        /// <summary>
        /// Add Mobile Services
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>        
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddMobileServices(this IServiceCollection services)
        {
            services.AddTransient<ISubmissionService, SubmissionService>();
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationHistoryService, NotificationHistoryService>();

            return services;
        }

        public static IServiceCollection ConfigureFirebaseSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FirebaseSettings>(configuration.GetSection("FirebaseSettings"));

            var firebaseSettings = configuration.GetSection("FirebaseSettings").Get<FirebaseSettings>();
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(firebaseSettings!.AdminSdkPath)
            });

            return services;
        }
    }
}
