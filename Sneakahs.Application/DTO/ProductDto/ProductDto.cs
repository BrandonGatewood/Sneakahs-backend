using System;

namespace Sneakahs.Application.DTO.ProductDto
{
    public class ProductDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required string ImageUrl { get; set; }

    }
}