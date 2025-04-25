using System;
using System.Collections.Generic;
using Sneakahs.Domain.Common;
using Sneakahs.Domain.Entities;

/*
    Relationship Types:
    One-to-One: Cart -> User (One cart belongs to a User)
    One-to-Many: Cart -> CartItem (A Cart can have many CartItems)
*/
namespace Sneakahs.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public required User User { get; set; }  // Navigation Property User

        // Navigation property for CartItems
        public required ICollection<CartItem> CartItems { get; set; }

        public decimal TotalAmount 
        {
            get 
            {
                return CartItems?.Sum(item => item.Quantity * item.Product.Price) ?? 0m;
            }
        }    
    }
}