namespace Explosion.API.DTOs
{
    public class CartResponseDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemResponseDTO> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public int TotalItems { get; set; }
    }
}
