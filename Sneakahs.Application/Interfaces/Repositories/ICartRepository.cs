using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCart(Guid userId);
        Task AddNewCartItem(CartItem newCartItem);
        Task UpdateCartItem(CartItem cartItem);
        Task RemoveCartItem(CartItem cartItem);
        Task Update(Cart updatedCart);
        Task CreateCart(Cart newCart);
    }
}