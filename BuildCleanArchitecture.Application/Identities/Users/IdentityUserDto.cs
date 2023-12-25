using BuildCleanArchitecture.Application.Common.Models;

namespace BuildCleanArchitecture.Application.Identities.Users
{
    public class IdentityUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Boolean Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }

    public class UserPagingFilterModel : PagingFilterModel
    {
        public string? SearchText { get; set; }
    }
}
