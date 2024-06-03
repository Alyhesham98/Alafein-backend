using Core;
using Core.Entities.Identity;
using Core.Interfaces.Alert.Repositories;
using Core.Interfaces.Event.Repositories;
using Core.Interfaces.Identity.Repositories;
using Core.Interfaces.LookUps.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Contexts;
using Presistence.Repositories.Alert;
using Presistence.Repositories.Event;
using Presistence.Repositories.Identity;
using Presistence.Repositories.LookUps;

namespace Presistence.Configuration
{
    public static class PersistenceDependencies
    {
        /// <summary>
        /// Add Db Connections
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>
        /// <param name="configuration">IConfiguration to access jwt settings</param>
        /// <param name="connectionStringName">Connection string name</param>
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                        IConfiguration configuration,
                                                        string connectionStringName)
        {
            services.AddDbContext<ApplicationDbContext>(
                        options =>
                        {
                            options.UseSqlServer(configuration.GetConnectionString(connectionStringName),
                                    sqlServerOptionsAction: sqlOptions =>
                                    {
                                        sqlOptions.EnableRetryOnFailure(
                                            maxRetryCount: 10,
                                            maxRetryDelay: TimeSpan.FromSeconds(30),
                                            errorNumbersToAdd: null);
                                    });
                        });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());


            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IFacilityRepository, FacilityRepository>();
            services.AddTransient<IOrganizerRepository, OrganizerRepository>();
            services.AddTransient<IVenueRepository, VenueRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddTransient<IVenueFacilityRepository, VenueFacilityRepository>();
            services.AddTransient<IBranchRepository, BranchRepository>();
            services.AddTransient<IWorkDayRepository, WorkDayRepository>();
            services.AddTransient<ISubmissionRepository, SubmissionRepository>();
            services.AddTransient<ISubmissionDateRepository, SubmissionDateRepository>();
            services.AddTransient<IFavouriteSubmissionRepository, FavouriteSubmissionRepository>();
            services.AddTransient<ISubmissionCommentRepository, SubmissionCommentRepository>();
            services.AddTransient<IBlockedCommentRepository, BlockedCommentRepository>();
            services.AddTransient<IUserOTPRepository, UserOTPRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<INotificationHistoryRepository, NotificationHistoryRepository>();
            services.AddTransient<IUserDeviceRepository, UserDeviceRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Add Identity user
        /// </summary>
        /// <param name="services">IServiceCollection to Extend</param>
        /// <returns>Extended IServiceCollection</returns>
        public static IServiceCollection AddIdentitySetup(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}
