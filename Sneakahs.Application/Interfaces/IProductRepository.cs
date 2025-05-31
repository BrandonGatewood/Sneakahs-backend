using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProduct(Guid id);
    }
}