using Explosion.API.DTOs;
using Explosion.API.Data;
using Explosion.API.Models;
using Explosion.API.Repositories;
using Explosion.API.Common;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            var cart = GetOrCreateCart(userId);
            return MapCart(cart);
        }

        public CartResponseDTO AddItem(int userId, AddCartItemDTO dto)
        {
            var cart = GetOrCreateCart(userId);
            var product = GetRequiredProduct(dto.ProductId);
            var existingItem = _cartRep.GetByCartAndProduct(cart.Id, dto.ProductId);

            if (existingItem is null)
            {
                ValidateStock(product, dto.Quantity);

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
                ValidateStock(product, newQty);

                existingItem.Quantity = newQty;
                _cartRep.UpdateItem(existingItem);
            }

            _cartRep.UpdateCart(cart);
            return MapCart(GetOrCreateCart(userId));
        }

        public CartResponseDTO UpdateItem(int userId, int itemId, UpdateCartItemDTO dto)
        {
            var cart = GetRequiredCart(userId);
            var item = GetRequiredOwnedItem(cart, itemId);
            var product = GetRequiredProduct(item.ProductId);

            ValidateStock(product, dto.Quantity);
            item.Quantity = dto.Quantity;

            _cartRep.UpdateItem(item);
            _cartRep.UpdateCart(cart);

            return MapCart(GetRequiredCart(userId));
        }

        public CartResponseDTO RemoveItem(int userId, int itemId)
        {
            var cart = GetRequiredCart(userId);
            var item = GetRequiredOwnedItem(cart, itemId);

            _cartRep.RemoveItem(item);
            _cartRep.UpdateCart(cart);

            return MapCart(GetRequiredCart(userId));
        }

        public CartResponseDTO Clear(int userId)
        {
            var cart = GetOrCreateCart(userId);

            _cartRep.ClearCart(cart.Id);
            _cartRep.UpdateCart(cart);

            return MapCart(GetOrCreateCart(userId));
        }

        public CheckoutResponseDTO Checkout(int userId)
        {
            using var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                var cart = _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(c => c.UserId == userId)
                    ?? throw new KeyNotFoundException("Carrinho nao encontrado");

                if (cart.Items.Count == 0)
                    throw new InvalidOperationException(CheckoutMessage.EmptyCart);

                foreach (var item in cart.Items)
                {
                    var product = item.Product
                        ?? throw new KeyNotFoundException("Produto nao encontrado no carrinho");

                    ValidateStock(
                        product,
                        item.Quantity,
                        $"{CheckoutMessage.InsufficientStockPrefix} para o produto {product.Name}");
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
                    Status = CheckoutStatus.Success,
                    TotalItems = totalItems,
                    TotalAmount = totalAmount,
                    Message = CheckoutMessage.CheckoutSuccess
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private Cart GetOrCreateCart(int userId)
        {
            return _cartRep.GetByUserId(userId) ?? _cartRep.CreateForUser(userId);
        }

        private Cart GetRequiredCart(int userId)
        {
            return _cartRep.GetByUserId(userId) ?? throw new KeyNotFoundException("Carrinho nao encontrado");
        }

        private Product GetRequiredProduct(int productId)
        {
            return _productRep.GetById(productId) ?? throw new KeyNotFoundException("Produto nao encontrado");
        }

        private CartItem GetRequiredOwnedItem(Cart cart, int itemId)
        {
            var item = _cartRep.GetItemById(itemId) ?? throw new KeyNotFoundException("Item nao encontrado no carrinho");

            if (item.CartId != cart.Id)
                throw new InvalidOperationException("Item nao pertence ao carrinho");

            return item;
        }

        private static void ValidateStock(Product product, int quantity, string? message = null)
        {
            if (product.Stock < quantity)
                throw new InvalidOperationException(message ?? CheckoutMessage.InsufficientStockPrefix);
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
