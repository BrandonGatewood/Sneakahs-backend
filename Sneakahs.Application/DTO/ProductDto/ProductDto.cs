namespace Sneakahs.Application.DTO.ProductDto
{
    public class ProductDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<ProductSizeDto.ProductSizeDto> Sizes { get; set; } = []; 
    }
}