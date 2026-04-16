using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.DTOs;

namespace Explosion.API.Services
{
    public class ProductServ
    {
        private readonly ProductRep _repository;

        public ProductServ(ProductRep repository)
        {
            _repository = repository;
        }
        public List<Product> ListEm()
        {
            return _repository.ListEm();
        }
        public Product? SearchId(int id)
        {
            return _repository.SearchId(id);
        }
        public Product? SearchName(string name)
        {
            return _repository.SearchName(name);
        }
        public Product Create(ProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image,
                Stock = dto.Stock,
            };
            return _repository.Create(product);
        }
        public Product? Update(int id, ProductDTO dto)
        {
            var product = _repository.SearchId(id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Stock = dto.Stock;
            product.Price = dto.Price;
            product.Image = dto.Image;
        
            return _repository.Update(product);
        }
        public bool Remove(int id)
        {
            var product = _repository.SearchId(id);
            if(product == null) return false;
            
            _repository.Remove(id);
            return true;
        }
        public bool FinishBuy(int id, int Stock)
        {
            var product = _repository.SearchId(id);
            if(product == null) return false;
            if (product.Stock < Stock)
            {
                throw new InvalidOperationException("Stock insuficiente");
            }
            product.Stock -= Stock;
            _repository.Update(product);
            return true;
        }
    }
}

