using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Identities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildCleanArchitecture.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AuthsController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUser _user;

        public AuthsController(IAuthService authService,
            IUser user)
        {
            _authService = authService;
            _user = user;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] LoginRequest user)
        {
            var result = await _authService.LoginAsync(user);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

    }
}
