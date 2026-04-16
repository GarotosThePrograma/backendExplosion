using Explosion.API.DTOs;
using Explosion.API.Repositories;
using Explosion.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UserController : ApiControllerBase
    {
        private readonly UserServ _service;
        private readonly UserRep _repository;

        public UserController(UserServ service, UserRep rep)
        {
            _service = service;
            _repository = rep;
        }

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                return Ok(_service.List());
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _service.GetById(id);
                if (user == null) return NotFound(new { message = "Usuario nao encontrado" });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost]
        public IActionResult Create(UserDTO dto)
        {
            try
            {
                var user = _service.Create(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UserDTO dto)
        {
            try
            {
                var user = _service.Update(id, dto);
                if (user == null) return NotFound(new { message = "Usuario nao encontrado" });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            try
            {
                var result = _service.Remove(id);
                if (!result) return NotFound(new { message = "Usuario nao encontrado" });
                return Ok(new { message = "Usuario removido com sucesso" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPatch("promote/{id:int}")]
        [HttpPatch("promover/{id:int}")]
        public IActionResult PromoteAdmin(int id)
        {
            try
            {
                var user = _repository.GetById(id);
                if (user == null) return NotFound(new { message = "Usuario nao encontrado" });
                user.Role = "Admin";
                _repository.Update(user);
                return Ok(new { message = "O usuario agora e administrador" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }
    }
}
