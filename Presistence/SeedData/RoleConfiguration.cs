using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.SeedData
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Id = "12fb541f-24f6-450a-86b6-8747fb8c3ff0", Name = "Super Admin", NormalizedName = "SUPER ADMIN", CreatedBy = "Alafein System", CreatedAt = new DateTime(2023, 11, 8, 0, 0, 0) },
                new Role { Id = "cbbcbc2d-b9a2-43a3-8894-576e4d2351e1", Name = "Admin", NormalizedName = "ADMIN", CreatedBy = "Alafein System", CreatedAt = new DateTime(2023, 11, 8, 0, 0, 0) },
                new Role { Id = "a8e2f362-5169-4c72-9030-5456f43d7826", Name = "Audience", NormalizedName = "AUDIENCE", CreatedBy = "Alafein System", CreatedAt = new DateTime(2023, 11, 8, 0, 0, 0) },
                new Role { Id = "e35a5541-be51-44a2-959a-f957d1142e3b", Name = "Host Venue", NormalizedName = "HOST VENUE", CreatedBy = "Alafein System", CreatedAt = new DateTime(2023, 11, 8, 0, 0, 0) }
            );
        }
    }
}
