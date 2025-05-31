using Sneakahs.Domain.Common;

/*
    add a list of sizes
    whats the difference between ICollection and List

    so far this needs to be implmemnted in the following files:
    CartService.cs
    ProductService.cs

*/
namespace Sneakahs.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<ProductSize> Sizes { get; set; } = [];

        public int GetAvailableQuantityForSize(decimal size)
        {
            ProductSize? productSize = Sizes.FirstOrDefault(s => s.Size == size) ?? throw new InvalidOperationException($"Size {size} is not available for this product.");
            return productSize.Quantity;
        }
    }
}