using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCart(Guid userId);
        Task AddNewCartItem(CartItem newCartItem);
        Task UpdateCartItem(CartItem cartItem);
        Task Update(Cart updatedCart);
        Task Create(Cart newCart);
    }
}