using Microsoft.AspNetCore.Identity;

namespace BuildCleanArchitecture.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}
