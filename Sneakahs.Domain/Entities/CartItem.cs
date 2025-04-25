using System;
using Sneakahs.Domain.Common;
using Sneakahs.Domain.Entities;

/*
    Relationship Types:
    Many-to-One: CartItem -> Cart (Each CartItem belongs in one Order)
    Many-to-One: CartItem -> Product (Each CartItem references one Product)
*/
namespace Sneakahs.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }
        public required Cart Cart { get; set; }  // Navigation property for Cart

        public Guid ProductId { get; set; }
        public required Product Product { get; set; }    // Navigation property for Product

        public int Quantity { get; set; }
    }
}