using BuildCleanArchitecture.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BuildCleanArchitecture.Infrastructure.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.HasKey(x => new { x.RoleId, x.UserId });
            builder.HasData(
                new ApplicationUserRole
                {
                    RoleId = Guid.Parse("cbc43a8e-f7bb-4445-baaf-1add431ffbbf"),
                    UserId = Guid.Parse("8e445865-a24d-4543-a6c6-9443d048cdb9")
                },
                new ApplicationUserRole
                {
                    RoleId = Guid.Parse("cac43a6e-f7bb-4448-baaf-1add431ccbbf"),
                    UserId = Guid.Parse("9e224968-33e4-4652-b7b7-8574d048cdb9")
                }
            );
        }
    }
}
