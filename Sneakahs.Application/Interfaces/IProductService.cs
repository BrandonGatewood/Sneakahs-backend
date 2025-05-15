using Sneakahs.Application.DTO.ProductDto;

namespace Sneakahs.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> GetProductById(Guid id);
    }
}