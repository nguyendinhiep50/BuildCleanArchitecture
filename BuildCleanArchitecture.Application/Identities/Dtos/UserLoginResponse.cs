namespace BuildCleanArchitecture.Application.Identities.Dtos
{
    public class UserLoginResponse
    {
        public string UserName { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}