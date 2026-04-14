namespace Explosion.API.DTO{
    public class productDTO{
        public string Nome{get;set;} = string.Empty;
        public decimal Preco{get;set;}
        public string Imagem{get;set;} = string.Empty;
        public int Estoque{get;set;}
        public string Descricao{get;set;} = string.Empty;
    }
}
