namespace Sneakahs.Application.DTO.CartItemDto
{
    public class CartItemRequestDto
    {
        public required Guid ProductId { get; set; }
        public decimal Size { get; set; }
        public required int Quantity { get; set; }
    }
}
