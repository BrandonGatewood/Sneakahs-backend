using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProduct(Guid id);
    }
}