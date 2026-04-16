using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Explosion.API.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected int GetAuthenticatedUserId()
        {
            var claimValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(claimValue) || !int.TryParse(claimValue, out var userId))
                throw new UnauthorizedAccessException("Usuario nao autenticado");

            return userId;
        }

        protected IActionResult HandleApiException(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => Unauthorized(new { message = ex.Message }),
                KeyNotFoundException => NotFound(new { message = ex.Message }),
                InvalidOperationException => BadRequest(new { message = ex.Message }),
                _ => StatusCode(500, new { message = "Erro interno no servidor" })
            };
        }
    }
}
