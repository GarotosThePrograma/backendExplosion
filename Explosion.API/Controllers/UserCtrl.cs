using Explosion.API.DTO;
using Explosion.API.Repositories;
using Explosion.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserServ _service;
        private readonly UsersRep _repository;

        public UserController(UserServ service, UsersRep rep)
        {
            _service = service;
            _repository = rep;
        }

        [HttpGet]
        public IActionResult ListEm()
        {
            return Ok(_service.ListEm());
        }

        [HttpGet("{id}")]
        public IActionResult SearchId(int id)
        {
            var user = _service.SearchId(id);
            if (user == null) return NotFound("Usuário não encontrado");
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(userDTO dto)
        {
            var user = _service.Create(dto);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, userDTO dto)
        {
            var user = _service.Update(id, dto);
            if (user == null) return NotFound("Usuário não encontrado");
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var result = _service.Remove(id);
            if (!result) return NotFound("Usuário não encontrado");
            return Ok("Usuário removido com sucesso");
        }
        [HttpPatch("promover/{id}")]
        public IActionResult PromoteADM(int id)
        {
            var user = _repository.SearchUserId(id);
            if (user == null) return NotFound("Usuário não encontrado");
            user.Role = "Admin";
            _repository.Update(user);
            return Ok("O usuário agora é administrador" );
        }
    }
}
