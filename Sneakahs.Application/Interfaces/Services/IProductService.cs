using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.ProductDto;

namespace Sneakahs.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsDto();
        Task<Result<ProductDto>> GetProductDto(Guid id);
    }
}