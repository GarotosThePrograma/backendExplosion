using Explosion.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    [Authorize]
    public class FavoriteController : ApiControllerBase
    {
        private readonly FavoriteServ _service;

        public FavoriteController(FavoriteServ service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult ListMine()
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var favorites = _service.ListMine(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost("{productId:int}")]
        public IActionResult Add(int productId)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                _service.Add(userId, productId);
                return Ok(new { message = "Produto adicionado aos favoritos" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpDelete("{productId:int}")]
        public IActionResult Remove(int productId)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var removed = _service.Remove(userId, productId);

                if (!removed) return NotFound(new { message = "Favorito nao encontrado" });
                return Ok(new { message = "Produto removido dos favoritos" });
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }
    }
}
