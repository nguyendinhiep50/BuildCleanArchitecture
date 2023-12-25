using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Application.Identities.Dtos
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
