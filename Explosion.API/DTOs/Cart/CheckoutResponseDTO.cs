namespace Explosion.API.DTOs
{
    public class CheckoutResponseDTO
    {
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
