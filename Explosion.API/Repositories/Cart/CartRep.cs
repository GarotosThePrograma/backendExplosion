using Explosion.API.Data;
using Explosion.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Explosion.API.Repositories
{
    public class CartRep
    {
        private readonly ExpDbContext _context;
        public CartRep(ExpDbContext context)
        {
            _context = context;
        }
        public Cart? GetByUserId(int userId)
        {
            return _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == userId);
        }
        public Cart CreateCart(int userId)
        {
            var cart = new Cart
            {
                UserId = userId
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart;
        }
                public CartItem? GetItemById(int itemId)
        {
            return _context.CartItems
                .Include(i => i.Product)
                .FirstOrDefault(i => i.Id == itemId);
        }
        public CartItem? GetItemCart(int cartId, int productId)
        {
            return _context.CartItems
                .FirstOrDefault(i => i.CartId == cartId && i.ProductId == productId);
        }
        public CartItem AddItem(CartItem item)
        {
            _context.CartItems.Add(item);
            _context.SaveChanges();
            return item;
        }
        public CartItem UpdateItem(CartItem item)
        {
            _context.CartItems.Update(item);
            _context.SaveChanges();
            return item;
        }
        public void RemoveItem(CartItem item)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
        public void ClearCart(int cartId)
        {
            var items = _context.CartItems.Where(i => i.CartId == cartId).ToList();
            if (items.Count == 0)return;

            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
        }
        public Cart UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
            return cart;
        }
    }
}