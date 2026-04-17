using System.ComponentModel.DataAnnotations;

namespace Explosion.API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

