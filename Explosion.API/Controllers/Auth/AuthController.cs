using Explosion.API.DTOs;
using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ApiControllerBase
    {
        private readonly UserRep _repository;
        private readonly TokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(
            UserRep repository,
            TokenService tokenService,
            IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var user = _repository.GetByEmail(dto.Email);
                if (user == null)
                    return Unauthorized(new { message = "Email ou senha invalidos" });

                var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
                var validHashedPassword = verifyResult == PasswordVerificationResult.Success ||
                                          verifyResult == PasswordVerificationResult.SuccessRehashNeeded;

                // Compatibilidade com usuarios antigos que ainda estejam com senha em texto puro.
                var validLegacyPlainText = user.Password == dto.Password;

                if (!validHashedPassword && !validLegacyPlainText)
                    return Unauthorized(new { message = "Email ou senha invalidos" });

                if (validLegacyPlainText)
                {
                    user.Password = _passwordHasher.HashPassword(user, dto.Password);
                    _repository.Update(user);
                }

                var token = _tokenService.CreateToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterDTO dto)
        {
            try
            {
                var userExist = _repository.GetByEmail(dto.Email);
                if (userExist != null)
                {
                    return BadRequest(new { message = "Esse email ja possui um usuario cadastrado" });
                }

                var user = new User
                {
                    Email = dto.Email,
                    Address = dto.Address,
                    Password = string.Empty,
                    Name = dto.Name,
                    Role = "User"
                };

                user.Password = _passwordHasher.HashPassword(user, dto.Password);
                _repository.Create(user);
                return Ok(new { message = "Usuario criado com sucesso" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }
    }
}
