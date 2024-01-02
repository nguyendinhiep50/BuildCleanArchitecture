using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Identities.Dtos;
using System.IdentityModel.Tokens.Jwt;
using BuildCleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
            var user = await _userManager.Users.Include(x=>x.UserRoles).ThenInclude(y=>y.Role).FirstOrDefaultAsync(x => x.UserName!.ToUpper() == args.UserName.ToUpper());

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
                JwtSecurityToken token;

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, _user.Id.ToString())
                    };

                if (_user.UserRoles != null)
                {
                    foreach (var role in user.UserRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role.Role.Name!));
                    }
                }

                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMonths(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );
                return new JwtSecurityTokenHandler().WriteToken(token);

            }
        }

    }
}

