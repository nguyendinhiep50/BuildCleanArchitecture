namespace BuildCleanArchitecture.Application.Identities.Users
{
    public class UserCreateDto : UserUpdateDto
    {
        public string Password { get; set; } = null!;
    }
}
