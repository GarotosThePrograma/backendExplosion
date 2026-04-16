// DTOs/ToggleFavoriteDTO.cs (opcional)
using System.ComponentModel.DataAnnotations;

namespace Explosion.API.DTOs
{
    public class ToggleFavoriteDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
    }
}
