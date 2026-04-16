using Explosion.API.DTOs;
using Explosion.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ApiControllerBase
    {
        private readonly ProductServ _service;

        public ProductController(ProductServ service)
        {
            _service = service;
        }

        [HttpGet]
        [HttpGet("productslist")]
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
                var product = _service.GetById(id);
                if (product == null) return NotFound(new { message = "Produto nao encontrado" });
                return Ok(product);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var product = _service.GetByName(name);
                if (product == null) return NotFound(new { message = "Produto nao encontrado" });
                return Ok(product);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost]
        [HttpPost("createproduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductDTO dto)
        {
            try
            {
                var product = _service.Create(dto);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPut("{id:int}")]
        [HttpPut("update/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, ProductDTO dto)
        {
            try
            {
                var product = _service.Update(id, dto);
                if (product == null) return NotFound(new { message = "Produto nao encontrado" });
                return Ok(product);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpDelete("{id:int}")]
        [HttpDelete("delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(int id)
        {
            try
            {
                var result = _service.Remove(id);
                if (!result) return NotFound(new { message = "Produto nao encontrado" });
                return Ok(new { message = "Produto removido com sucesso" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost("{id:int}/buy")]
        [HttpPost("{id:int}/comprar")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Buy(
            int id,
            [FromQuery(Name = "quantidade")] int? legacyQuantity,
            [FromQuery] int? quantity)
        {
            try
            {
                var finalQuantity = quantity ?? legacyQuantity;
                if (finalQuantity is null || finalQuantity <= 0)
                    return BadRequest(new { message = "Quantidade invalida" });

                var result = _service.FinishBuy(id, finalQuantity.Value);
                if (!result) return NotFound(new { message = "Produto nao encontrado" });
                return Ok(new { message = "Compra finalizada com sucesso" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }
    }
}
