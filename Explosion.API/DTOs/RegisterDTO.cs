using System.ComponentModel.DataAnnotations;

namespace Explosion.API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public string Nome { get; set; } = string.Empty;
    }
}
