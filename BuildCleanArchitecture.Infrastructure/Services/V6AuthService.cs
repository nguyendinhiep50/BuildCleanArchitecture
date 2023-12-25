using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Application.Identities.Dtos;
using BuildCleanArchitecture.Application.Utilities;
using BuildCleanArchitecture.Domain.Entities;
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
        private readonly IApplicationDbContext _dbContext;

        public V6AuthService(
            IConfiguration configuration,
        IApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<UserLoginResponse> LoginAsync(LoginRequest args)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToUpper() == args.UserName.ToUpper());

            if (user != null)
            {
                // admin account
                if (user.UserName.Trim() == "V6")
                {
                    args.Password = "";
                }

                var passwordEncrypted = EnCryptPasswordUtilities.EnCrypt(args.UserName.ToUpper() + args.Password);

                if (user.Password == passwordEncrypted)
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

            string GetToken(User _user)
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
                        new Claim(ClaimTypes.NameIdentifier, _user.UserName),
                        new Claim(ClaimTypes.Sid, _user.UserId.ToString())

                    }),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            }
        }

    }
}

