using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };
            var Roles =await userManager.GetRolesAsync(user);
            foreach (var Role in Roles)
              claims.Add(new Claim(ClaimTypes.Role, Role));

            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurtyKey"]));

            var token = new JwtSecurityToken(
                    audience: _configuration["JWT:ValidAudience"],
                    issuer: _configuration["JWT:vaildIssuer"],
                    expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                    claims : claims,
                    signingCredentials :new SigningCredentials(authkey , SecurityAlgorithms.Aes128CbcHmacSha256)

                );
           return new JwtSecurityTokenHandler().WriteToken(token);

            
        }
    }
}
