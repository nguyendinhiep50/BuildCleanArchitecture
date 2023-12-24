using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = null!;
    public Boolean Status { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}
