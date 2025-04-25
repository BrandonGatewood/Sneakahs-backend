using BCrypt.Net;
using System;
using System.Collections.Generic;
using Sneakahs.Domain.Common;
using Sneakahs.Domain.Entities;

/*
    Relationship Types:
    One-to-Many: User -> Order (User can have many orders)
    One-to-One: User -> Cart (User can only have one cart)
*/
namespace Sneakahs.Domain.Entities
{
    public class User : BaseEntity
    {
        public required String Username { get; set; }
        public required String Email { get; set; }
        public required String HashedPassword { get; set; }
        public required Cart Cart { get; set; }  // Navigation Property User

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public void SetPassword(String rawPassword) {
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public bool CheckPassword(String rawPassword) {
            return BCrypt.Net.BCrypt.Verify(rawPassword, HashedPassword);
        }
    }
}