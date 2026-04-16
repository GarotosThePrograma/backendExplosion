using Explosion.API.DTOs;
using Explosion.API.Services;
using Explosion.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ApiControllerBase
    {
        private readonly CartServ _service;

        public CartController(CartServ service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetMyCart()
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var cart = _service.GetMyCart(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost("items")]
        public IActionResult AddItem([FromBody] AddCartItemDTO dto)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var cart = _service.AddItem(userId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPut("items/{itemId:int}")]
        public IActionResult UpdateItem(int itemId, [FromBody] UpdateCartItemDTO dto)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var cart = _service.UpdateItem(userId, itemId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpDelete("items/{itemId:int}")]
        public IActionResult RemoveItem(int itemId)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var cart = _service.RemoveItem(userId, itemId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpDelete("clear")]
        public IActionResult Clear()
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var cart = _service.Clear(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }

        [HttpPost("checkout")]
        public IActionResult Checkout()
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                var result = _service.Checkout(userId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = CheckoutStatus.Failed, message = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message == CheckoutMessage.EmptyCart)
            {
                return BadRequest(new { status = CheckoutStatus.Failed, message = ex.Message });
            }
            catch (InvalidOperationException ex) when (
                ex.Message.StartsWith(CheckoutMessage.InsufficientStockPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { status = CheckoutStatus.Failed, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = CheckoutStatus.Failed,
                    message = CheckoutMessage.CheckoutInternalError,
                    detail = ex.Message
                });
            }
        }
    }
}
