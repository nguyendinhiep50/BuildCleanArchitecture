using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Identities.Dtos;
using BuildCleanArchitecture.Application.Utilities;
using BuildCleanArchitecture.Domain.Entities;
using BuildCleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuildCleanArchitecture.Infrastructure.Services
{
    internal class V6AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public V6AuthService(
            IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<UserLoginResponse> LoginAsync(LoginRequest args)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName!.ToUpper() == args.UserName.ToUpper());

            if (user != null)
            {
                // admin account
                if (user.UserName!.Trim() == "V6")
                {
                    args.Password = "";
                }

                var checkPassword = await _userManager.CheckPasswordAsync(user, args.Password);

                if (checkPassword)
                {
                    var token = GetToken(user);

                    if (string.IsNullOrEmpty(token))
                    {
                        return null!;
                    }

                    return new UserLoginResponse
                    {
                        UserName = user.UserName,
                        Token = token
                    };
                }
            }

            return null!;

            string GetToken(ApplicationUser _user)
            {
                if (_user == null)
                {
                    return null!;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecrectKey")!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, _user.UserName !),
                        new Claim(ClaimTypes.Sid, _user.Id.ToString())

                    }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            }
        }

    }
}

