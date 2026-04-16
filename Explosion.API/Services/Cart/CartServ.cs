using Explosion.API.DTOs;
using Explosion.API.Data;
using Explosion.API.Models;
using Explosion.API.Repositories;

namespace Explosion.API.Services
{
    public class CartServ
    {
        private readonly CartRep _cartRep;
        private readonly ProductRep _productRep;
        private readonly ExpDbContext _context;

        public CartServ(CartRep cartRep, ProductRep productRep, ExpDbContext context)
        {
            _cartRep = cartRep;
            _productRep = productRep;
            _context = context;
        }

        public CartResponseDTO GetMyCart(int userId)
        {
            var cart = _cartRep.GetByUserId(userId) ?? _cartRep.CreateCart(userId);
            return MapCart(cart);
        }

        public CartResponseDTO AddItem(int userId, AddCartItemDTO dto)
        {
            var cart = _cartRep.GetByUserId(userId) ?? _cartRep.CreateCart(userId);

            var product = _productRep.SearchId(dto.ProductId)
                ?? throw new KeyNotFoundException("Produto nao encontrado");

            var existingItem = _cartRep.GetItemCart(cart.Id, dto.ProductId);

            if (existingItem is null)
            {
                if (product.Stock < dto.Quantity)
                    throw new InvalidOperationException("Estoque insuficiente");

                _cartRep.AddItem(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.IdProd,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price
                });
            }
            else
            {
                var newQty = existingItem.Quantity + dto.Quantity;
                if (product.Stock < newQty)
                    throw new InvalidOperationException("Estoque insuficiente");

                existingItem.Quantity = newQty;
                _cartRep.UpdateItem(existingItem);
            }

            _cartRep.UpdateCart(cart);
            var updated = _cartRep.GetByUserId(userId)!;
            return MapCart(updated);
        }

        public CartResponseDTO UpdateItem(int userId, int itemId, UpdateCartItemDTO dto)
        {
            var cart = _cartRep.GetByUserId(userId) ?? throw new KeyNotFoundException("Carrinho nao encontrado");
            var item = _cartRep.GetItemById(itemId) ?? throw new KeyNotFoundException("Item nao encontrado no carrinho");

            if (item.CartId != cart.Id)
                throw new InvalidOperationException("O item nao pertence ao carrinho");

            var product = _productRep.SearchId(item.ProductId)
                ?? throw new KeyNotFoundException("Produto nao encontrado");

            if (product.Stock < dto.Quantity)
                throw new InvalidOperationException("Estoque insuficiente");

            item.Quantity = dto.Quantity;
            _cartRep.UpdateItem(item);
            _cartRep.UpdateCart(cart);

            var updated = _cartRep.GetByUserId(userId)!;
            return MapCart(updated);
        }

        public CartResponseDTO RemoveItem(int userId, int itemId)
        {
            var cart = _cartRep.GetByUserId(userId) ?? throw new KeyNotFoundException("Carrinho nao encontrado");
            var item = _cartRep.GetItemById(itemId) ?? throw new KeyNotFoundException("Item nao encontrado no carrinho");

            if (item.CartId != cart.Id)
                throw new InvalidOperationException("Item nao pertence ao carrinho");

            _cartRep.RemoveItem(item);
            _cartRep.UpdateCart(cart);

            var updated = _cartRep.GetByUserId(userId)!;
            return MapCart(updated);
        }

        public CartResponseDTO Clear(int userId)
        {
            var cart = _cartRep.GetByUserId(userId) ?? _cartRep.CreateCart(userId);

            _cartRep.ClearCart(cart.Id);
            _cartRep.UpdateCart(cart);

            var updated = _cartRep.GetByUserId(userId)!;
            return MapCart(updated);
        }

        public CheckoutResponseDTO Checkout(int userId)
        {
            var cart = _cartRep.GetByUserId(userId) ?? throw new KeyNotFoundException("Carrinho nao encontrado");
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Carrinho vazio");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var item in cart.Items)
                {
                    var product = item.Product ?? _productRep.SearchId(item.ProductId);
                    if (product == null)
                        throw new KeyNotFoundException($"Produto {item.ProductId} nao encontrado");
                    item.Product = product;

                    if (product.Stock < item.Quantity)
                        throw new InvalidOperationException($"Estoque insuficiente para o produto {product.Name}");
                }

                var totalItems = 0;
                decimal totalAmount = 0;

                foreach (var item in cart.Items)
                {
                    var product = item.Product!;
                    product.Stock -= item.Quantity;
                    totalItems += item.Quantity;
                    totalAmount += item.UnitPrice * item.Quantity;
                }

                _context.CartItems.RemoveRange(cart.Items);
                _context.SaveChanges();
                transaction.Commit();

                return new CheckoutResponseDTO
                {
                    TotalItems = totalItems,
                    TotalAmount = totalAmount,
                    Message = "Checkout realizado com sucesso"
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private static CartResponseDTO MapCart(Cart cart)
        {
            var items = cart.Items.Select(i => new CartItemResponseDTO
            {
                CartItemId = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? string.Empty,
                Image = i.Product?.Image ?? string.Empty,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList();

            return new CartResponseDTO
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                Items = items,
                Subtotal = items.Sum(x => x.LineTotal),
                TotalItems = items.Sum(x => x.Quantity)
            };
        }
    }
}
