using Explosion.API.DTOs;
using Explosion.API.Models;
using Explosion.API.Repositories;

namespace Explosion.API.Services
{
    public class FavoriteServ
    {
        private readonly FavoriteRep _favoriteRep;
        private readonly ProductRep _productRep;

        public FavoriteServ(FavoriteRep favoriteRep, ProductRep productRep)
        {
            _favoriteRep = favoriteRep;
            _productRep = productRep;
        }

        public List<FavoriteResponseDTO> ListMine(int userId)
        {
            return _favoriteRep.ListByUserId(userId)
                .Select(f => new FavoriteResponseDTO
                {
                    FavoriteId = f.Id,
                    ProductId = f.ProductId,
                    ProductName = f.Product?.Name ?? string.Empty,
                    Price = f.Product?.Price ?? 0,
                    Image = f.Product?.Image ?? string.Empty
                })
                .ToList();
        }

        public void Add(int userId, int productId)
        {
            var product = _productRep.SearchId(productId)
                ?? throw new KeyNotFoundException("Produto nao encontrado");

            if (_favoriteRep.Exists(userId, productId))
                return;

            _favoriteRep.Add(new Favorite
            {
                UserId = userId,
                ProductId = product.IdProd,
            });
        }

        public bool Remove(int userId, int productId)
        {
            var fav = _favoriteRep.GetByUserAndProduct(userId, productId);
            if (fav is null) return false;

            _favoriteRep.Remove(fav);
            return true;
        }
    }
}
