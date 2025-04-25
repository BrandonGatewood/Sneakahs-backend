using System;
using Sneakahs.Domain.Common;
using Sneakahs.Domain.Entities;

/*
    Relationship Types:
    Many-to-One: OrderItem -> Order (Each OrderItem belongs in one Order)
    Many-to-One: OrderItem -> Product (Each OrderItem references one Product) 
*/
namespace Sneakahs.Domain.Entities
{
    public class OrderItem : BaseEntity {
        public Guid OrderId { get; set; }
        public required Order Order { get; set; }    // Navigation property for Order

        public Guid ProductId { get; set; }
        public required Product Product { get; set; }    // Navigation property for Product

        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}