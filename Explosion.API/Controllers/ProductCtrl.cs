using Explosion.API.DTO;
using Explosion.API.Services;
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

        [HttpGet]
        public IActionResult ListEm()
        {
            return Ok(_service.ListEm());
        }

        [HttpGet("{id}")]
        public IActionResult SearchId(int id)
        {
            var Product = _service.SearchId(id);
            if (Product == null) return NotFound("Produto não encontrado");
            return Ok(Product);
        }

        [HttpPost]
        public IActionResult Create(productDTO dto)
        {
            var Product = _service.Create(dto);
            return Ok(Product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, productDTO dto)
        {
            var product = _service.Update(id, dto);
            if (product == null) return NotFound("Produto não encontrado");
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var result = _service.Remove(id);
            if (!result) return NotFound("Produto não encontrado");
            return Ok("Produto removido com sucesso");
        }

        [HttpPost("{id}/comprar")]
        public IActionResult FinishBuy(int id, [FromQuery] int quantidade)
        {
            var result = _service.FinishBuy(id, quantidade);
            if (!result) return NotFound("Produto não encontrado");
            return Ok("Compra finalizada com sucesso");
        }
    }
}