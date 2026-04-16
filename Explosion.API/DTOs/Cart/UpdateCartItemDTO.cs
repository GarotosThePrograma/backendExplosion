// DTOs/UpdateCartItemDTO.cs
using System.ComponentModel.DataAnnotations;

namespace Explosion.API.DTOs
{
    public class UpdateCartItemDTO
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
