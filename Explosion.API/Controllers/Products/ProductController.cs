using Explosion.API.DTOs;
using Explosion.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductServ _service;

        public ProductController(ProductServ service)
        {
            _service = service;
        }

        [HttpGet("productslist")]
        public IActionResult ListEm()
        {
            return Ok(_service.ListEm());
        }

        [HttpGet("{id:int}")]
        public IActionResult SearchId(int id)
        {
            var product = _service.SearchId(id);
            if (product == null) return NotFound("Produto nao encontrado");
            return Ok(product);
        }

        [HttpGet("name/{name}")]
        public IActionResult SearchName(string name)
        {
            var product = _service.SearchName(name);
            if (product == null) return NotFound("Produto nao encontrado");
            return Ok(product);
        }

        [HttpPost("createproduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductDTO dto)
        {
            var product = _service.Create(dto);
            return Ok(product);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, ProductDTO dto)
        {
            var product = _service.Update(id, dto);
            if (product == null) return NotFound("Produto nao encontrado");
            return Ok(product);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(int id)
        {
            var result = _service.Remove(id);
            if (!result) return NotFound("Produto nao encontrado");
            return Ok("Produto removido com sucesso");
        }

        [HttpPost("{id}/comprar")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult FinishBuy(int id, [FromQuery] int quantidade)
        {
            try
            {
                var result = _service.FinishBuy(id, quantidade);
                if (!result) return NotFound("Produto nao encontrado");
                return Ok("Compra finalizada com sucesso");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
