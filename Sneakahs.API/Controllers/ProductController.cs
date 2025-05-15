using Microsoft.AspNetCore.Mvc;
using Sneakahs.Application.Interfaces;

namespace Sneakahs.API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var productDtos = await _productService.GetAllProducts();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getProductById(Guid id)
        {
            var productDto = await _productService.GetProductById(id);

            if (productDto == null)
            {
                return NotFound(new { message = "Product not found."});
            }
            return Ok(productDto);
        }
    }
}