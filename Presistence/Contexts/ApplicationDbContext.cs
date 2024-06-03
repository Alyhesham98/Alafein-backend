using Core.Entities.Alert;
using Core.Entities.BaseEntities;
using Core.Entities.Event;
using Core.Entities.Identity;
using Core.Entities.LookUps;
using Core.Interfaces.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Presistence.SeedData;
using System.Data;

namespace Presistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>, IApplicationDbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    IDateTimeService dateTime,
                                    IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public IDbConnection Connection => Database.GetDbConnection();

        #region Identity
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        #endregion

        #region LookUps
        public DbSet<Category> Categories { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        #endregion

        #region Event
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<VenueFacility> VenueFacilities { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmissionDate> SubmissionDates { get; set; }
        public DbSet<FavouriteSubmission> FavouriteSubmissions { get; set; }
        public DbSet<SubmissionComment> SubmissionComments { get; set; }
        public DbSet<BlockedComment> BlockedComments { get; set; }
        #endregion

        #region Alert
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationHistory> NotificationsHistory { get; set; }
        #endregion
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = _dateTime.NowUtc;
                    entry.Entity.CreatedBy = _authenticatedUser.UserId ?? "Website Alafein";
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedAt = _dateTime.NowUtc;
                    entry.Entity.LastModifiedBy = _authenticatedUser.UserId ?? "Website Alafein";
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,2 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            builder.Entity<FavouriteSubmission>()
                 .HasKey(p => new { p.SubmissionId, p.UserId });

            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(RoleConfiguration).Assembly);

            #region Set OnDeleteToRestrict
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.SetTableName(entityType.DisplayName());

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
            #endregion

            #region RenameIdentityTables
            builder.Entity<User>()
                .ToTable("User", "security")
                ;

            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Role", "security");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaim", "security");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("UserRoleClaims", "security");

            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "security");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "security");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "security");
            });
            #endregion
        }
    }
}
