using Explosion.API.DTO;
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
        //[Authorize(Roles ="Admin")]
        public IActionResult SearchId(int id)
        {
            var Product = _service.SearchId(id);
            if (Product == null) return NotFound("Produto não encontrado");
            return Ok(Product);
        }
        [HttpGet("name/{name}")]
        public IActionResult SearchName(string name)
        {
            var Product = _service.SearchName(name);
            if (Product == null) return NotFound("Produto não encontrado");
            return Ok(Product);
        }
        [HttpPost("createproduct")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create(productDTO dto)
        {
            var Product = _service.Create(dto);
            return Ok(Product);
        }

        [HttpPut("update/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Update(int id, productDTO dto)
        {
            var product = _service.Update(id, dto);
            if (product == null) return NotFound("Produto não encontrado");
            return Ok(product);
        }

        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "USER")]
        public IActionResult Remove(int id)
        {
            var result = _service.Remove(id);
            if (!result) return NotFound("Produto não encontrado");
            return Ok("Produto removido com sucesso");
        }

        [HttpPost("{id}/comprar")]
        //[Authorize(Roles = "User")]
        public IActionResult FinishBuy(int id, [FromQuery] int quantidade)
        {
            var result = _service.FinishBuy(id, quantidade);
            if (!result) return NotFound("Produto não encontrado");
            return Ok("Compra finalizada com sucesso");
        }
    }
}

