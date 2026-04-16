namespace Explosion.API.DTOs
{
    public class CheckoutResponseDTO
    {
        public string Status { get; set; } = "success"; // success | failed
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
