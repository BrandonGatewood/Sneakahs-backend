using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Interfaces.Services;

namespace Sneakahs.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var productDtos = await _productService.GetAllProductsDto();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var productDto = await _productService.GetProductDto(id);

            if (productDto == null)
            {
                return NotFound(new { message = "Product not found."});
            }
            return Ok(productDto);
        }
    }
}