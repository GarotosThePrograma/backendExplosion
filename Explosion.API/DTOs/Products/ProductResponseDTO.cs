namespace Explosion.API.DTOs
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
