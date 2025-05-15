using System.Reflection.Metadata.Ecma335;
using Sneakahs.Application.DTO.ProductDto;
using Sneakahs.Application.Interfaces;

namespace Sneakahs.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();

            return products.Select(p => new ProductDto
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl
            });
        }

        public async Task<ProductDto?> GetProductById(Guid id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl
            };
        }
    }
}