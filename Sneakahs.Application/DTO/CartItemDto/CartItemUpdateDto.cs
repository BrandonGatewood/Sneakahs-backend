namespace Sneakahs.Application.DTO.CartItemDto
{
    public class CartItemUpdateDto
    {
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}