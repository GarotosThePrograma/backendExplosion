using System.ComponentModel.DataAnnotations;

namespace Explosion.API.Models
{
    public class Product
    {
        [Key]
        public int IdProd { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string Imagem { get; set; }
        public int Tipo { get; set; }
        public string Descricao { get; set; }
    }
}