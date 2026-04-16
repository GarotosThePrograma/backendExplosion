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
            var jwtKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("A chave JWT (Jwt:Key) não foi configurada.");
            var issuer = _config["Jwt:Issuer"]
                ?? throw new InvalidOperationException("O emissor JWT (Jwt:Issuer) não foi configurado.");
            var audience = _config["Jwt:Audience"]
                ?? throw new InvalidOperationException("A audiência JWT (Jwt:Audience) não foi configurada.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var accessTokenMinutes = _config.GetValue<int?>("Jwt:AccessTokenMinutes") ?? 480;

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims:claims,
                notBefore: DateTime.UtcNow,
                expires:DateTime.UtcNow.AddMinutes(accessTokenMinutes),
                signingCredentials:creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
