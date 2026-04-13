using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Explosion.API.Models;
using Microsoft.IdentityModel.Tokens;


namespace Explosion.API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuers"],
                audience: _config["Jwt:Audience"],
                claims:claims,
                expires:DateTime.Now.AddHours(8),
                signingCredentials:creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}