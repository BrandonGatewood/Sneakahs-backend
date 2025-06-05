using Sneakahs.Domain.Entities;

namespace Sneakahs.Application.DTO.OrderDto
{
    public class OrderDto
    {
        public required ICollection<OrderItemDto.OrderItemDto> OrderItems { get; set; } = [];
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }


        public required string Status { get; set; }
        public required DateTime? PaidAt { get; set; }
        public required DateTime? ShippedAt { get; set; }

        public required ShippingAddress ShippingAddress { get; set; }
        public required PaymentDetails PaymentDetails { get; set; }
    }
}