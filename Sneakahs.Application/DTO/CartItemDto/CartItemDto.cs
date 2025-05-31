namespace Sneakahs.Application.DTO.CartItemDto
{
    public class CartItemDto
    {
        public required Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }
        public required int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }
}