using System.ComponentModel.DataAnnotations;

namespace Explosion.API.DTOs
{
    public class AddCartItemDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
