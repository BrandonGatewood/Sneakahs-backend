using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Interfaces.Services;

namespace Sneakahs.API.Controllers
{
    /// <summary>
    /// Handles product operations such as getting a product and all products.
    /// </summary>
    [Route("[Controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        /// Gets all Products available.
        /// </summary>
        /// <returns>200 OK with a productDto.</returns> 
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            // Validate request and get all products
            var productDtos = await _productService.GetAllProductsDto();

            return Ok(productDtos);
        }

        /// <summary>
        /// Get a product if available.
        /// </summary>
        /// <param name="id">productId data (id).</param>
        /// <returns>200 OK with a productDto if successful; otherwise 404 Not Found.</returns> 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            // Validate request and get a product
            var productDto = await _productService.GetProductDto(id);

            // Get failed
            if (productDto == null)
                return NotFound(new { message = "Product not found." });

            return Ok(productDto);
        }
    }
}