using Microsoft.EntityFrameworkCore;
using Sneakahs.Application.Interfaces.Repositories;
using Sneakahs.Domain.Entities;
using Sneakahs.Persistence.Data;

/*
    CartRepository interacts with the database using Entity Framework Core.
    It implements ICartRepository interface and provides methods to:
    - Get a Cart by userId
    - Update Cart with updatedCart
*/
namespace Sneakahs.Infrastructure.Repositories
{
    public class CartRepository(ApplicationDbContext context) : ICartRepository
    {
        private readonly ApplicationDbContext _context = context;

        // Get a Cart by the userId
        // Includes the CartItems and their associated Products to avoid lazy loading.
        public async Task<Cart?> GetCart(Guid userId)
        {
            return await _context.Carts
            .Include(c => c.CartItems).ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // Add new CartItem to User's Cart 
        public async Task AddNewCartItem(CartItem newCartItem)
        {
            await _context.CartItems.AddAsync(newCartItem);
            await _context.SaveChangesAsync();
        }

        // Update exisiting CartItem in Users Cart
        public async Task UpdateCartItem(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        // Delete CartItem from Users Cart
        public async Task RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        // Update existing User's Cart with updatedCart
        public async Task Update(Cart updatedCart)
        {
            _context.Carts.Update(updatedCart);
            await _context.SaveChangesAsync();
        }

        // Create a new Cart for a new User
        public async Task CreateCart(Cart newCart)
        {
            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();
        }
    }
}