using BuildCleanArchitecture.Application.Identities.Dtos;

namespace BuildCleanArchitecture.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> LoginAsync(LoginRequest user);
    }
}
