using BuildCleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildCleanArchitecture.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = Guid.Parse("cac43a6e-f7bb-4448-baaf-1add431ccbbf"),
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new ApplicationRole
                {
                    Id = Guid.Parse("cbc43a8e-f7bb-4445-baaf-1add431ffbbf"),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );
        }
    }
}
