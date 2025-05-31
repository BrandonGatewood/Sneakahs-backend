using Sneakahs.Application.Common;
using Sneakahs.Application.DTO.ProductDto;
using Sneakahs.Application.DTO.ProductSizeDto;
using Sneakahs.Application.Interfaces;
using Sneakahs.Domain.Entities;

namespace Sneakahs.Infrastructure.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<ProductDto>> GetAllProductsDto()
        {
            IEnumerable<Product> products = await _productRepository.GetAllProducts();

            return [.. products.Select(ToDto)];
        }

        public async Task<Result<ProductDto>> GetProductDto(Guid id)
        {
            Product? product = await _productRepository.GetProduct(id);

            if (product == null)
                return Result<ProductDto>.Fail("Product not found");

            return Result<ProductDto>.Ok(ToDto(product));
        }

        // ------------- Helper Functions -------------
        private static ProductDto ToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Sizes = [.. product.Sizes.Select(p => new ProductSizeDto{
                    Size = p.Size,
                    Quantity = p.Quantity
                })]
            };
        }
    }
}