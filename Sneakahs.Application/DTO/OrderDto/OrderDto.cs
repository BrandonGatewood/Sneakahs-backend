namespace Sneakahs.Application.DTO.OrderDto
{
    public class OrderDto
    {
        public required string PaymentIntentId { get; set; }
        public required ICollection<OrderItemDto.OrderItemDto> OrderItems { get; set; } = [];
        public decimal Tax { get; set; }
        public decimal Total { get; set; }


        public required string Status { get; set; }
        public required DateTime? PaidAt { get; set; }
        public required DateTime? ShippedAt { get; set; }

        public required ShippingAddressDto.ShippingAddressDto ShippingAddressDto { get; set; }
    }
}