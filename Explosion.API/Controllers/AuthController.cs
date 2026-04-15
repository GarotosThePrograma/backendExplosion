using Explosion.API.DTOs;
using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsersRep _repository;
        private readonly TokenService _tokenService;

        public AuthController(UsersRep repository, TokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _repository.SearchUserEmail(dto.Email);
            if (user == null || user.Password != dto.Password)
                return Unauthorized("Email ou senha invalidos");

            var token = _tokenService.CreateToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterDTO dto)
        {
            var userExist = _repository.SearchUserEmail(dto.Email);
            if (userExist != null)
            {
                return BadRequest("Esse email ja possui um usuario cadastrado");
            }

            var user = new User
            {
                Email = dto.Email,
                Address = dto.Address,
                Password = dto.Password,
                Name = dto.Name,
                Role = "User"
            };

            _repository.Create(user);
            return Ok("Usuario criado com sucesso");
        }
    }
}


