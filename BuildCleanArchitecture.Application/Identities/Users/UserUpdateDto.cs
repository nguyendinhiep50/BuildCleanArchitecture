namespace BuildCleanArchitecture.Application.Identities.Users
{
    public class UserUpdateDto
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public Boolean Status { get; set; }

    }
}
