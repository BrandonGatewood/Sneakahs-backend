namespace Sneakahs.Application.DTO.CartDto
{
    public class CartDto
    {
        public required ICollection<CartItemDto.CartItemDto> CartItems { get; set; }
        public required int Quantity { get; set; }
        public decimal Total { get; set; } 
    }
}