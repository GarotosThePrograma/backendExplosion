using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.DTO;

namespace Explosion.API.Services
{
    public class ProductServ
    {
        private readonly ProductRep _repository;

        public ProductServ(ProductRep repository)
        {
            repository = _repository;
        }
        public List<Product> ListEm()
        {
            return _repository.ListEm();
        }
        public Product? SearchId(int id)
        {
            return _repository.SearchId(id);
        }
        public Product Create(productDTO dto)
        {
            var product = new Product
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                Imagem = dto.Imagem,
                Estoque = dto.Estoque,
                IdProd = dto.IdProd
            };
            return _repository.Create(product);
        }
        public Product? Update(int id, productDTO dto)
        {
            var product = _repository.SearchId(id);
            if (product == null) return null;

            product.Nome = dto.Nome;
            product.IdProd = dto.IdProd;
            product.Descricao = dto.Descricao;
            product.Estoque = dto.Estoque;
            product.Preco = dto.Preco;
            product.Imagem = dto.Imagem;
        
            return _repository.Update(product);
        }
        public bool Remove(int id)
        {
            var product = _repository.SearchId(id);
            if(product == null) return false;
            
            _repository.Remove(id);
            return true;
        }
        public bool FinishBuy(int id, int estoque)
        {
            var product = _repository.SearchId(id);
            if(product == null) return false;
            if (product.Estoque < estoque)
            {
                throw new Exception("Estoque insuficiente");
            }
            product.Estoque -= estoque;
            _repository.Update(product);
            return true;
        }
    }
}