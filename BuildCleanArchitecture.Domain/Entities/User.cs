namespace BuildCleanArchitecture.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string? Password { get; set; }

    }
}
