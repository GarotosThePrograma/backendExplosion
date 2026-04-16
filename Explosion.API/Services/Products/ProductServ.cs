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

        public List<ProductResponseDTO> List()
        {
            return _repository.List().Select(MapToResponse).ToList();
        }

        public ProductResponseDTO? GetById(int id)
        {
            var product = _repository.GetById(id);
            return product is null ? null : MapToResponse(product);
        }

        public ProductResponseDTO? GetByName(string name)
        {
            var product = _repository.GetByName(name);
            return product is null ? null : MapToResponse(product);
        }

        public ProductResponseDTO Create(ProductDTO dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Image = dto.Image,
                Stock = dto.Stock,
            };

            return MapToResponse(_repository.Create(product));
        }

        public ProductResponseDTO? Update(int id, ProductDTO dto)
        {
            var product = _repository.GetById(id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Stock = dto.Stock;
            product.Price = dto.Price;
            product.Image = dto.Image;

            return MapToResponse(_repository.Update(product));
        }

        public bool Remove(int id)
        {
            var product = _repository.GetById(id);
            if (product == null) return false;

            _repository.Remove(id);
            return true;
        }

        public bool FinishBuy(int id, int quantity)
        {
            var product = _repository.GetById(id);
            if (product == null) return false;

            if (product.Stock < quantity)
            {
                throw new InvalidOperationException("Stock insuficiente");
            }

            product.Stock -= quantity;
            _repository.Update(product);
            return true;
        }

        private static ProductResponseDTO MapToResponse(Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.IdProd,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Image = product.Image,
                Type = product.Tipo,
                Description = product.Description
            };
        }
    }
}
