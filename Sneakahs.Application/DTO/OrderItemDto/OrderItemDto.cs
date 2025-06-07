namespace Sneakahs.Application.DTO.OrderItemDto
{
    public class OrderItemDto
    {
        public required Guid Id { get; set; }
        public required string ProductName { get; set; } 
        public required string ProductImgUrl { get; set; } 

        public required int Quantity { get; set; }
        public decimal Size { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}