using System.ComponentModel.DataAnnotations;

namespace Explosion.API.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Role{get;set;}
    }
}