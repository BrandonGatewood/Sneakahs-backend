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
        public String Username { get; private set; }
        public String Email { get; private set; }
        public String HashedPassword { get; private set; }
        public Cart Cart { get; private set; }  // Navigation Property User

        public ICollection<Order> Orders { get; private set; } = new List<Order>();

        public User() {}

        // Constructor to initialize the required fields
        public User(string username, string email, string rawPassword)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(rawPassword)) throw new ArgumentException("Password cannot be empty.");

            Username = username;
            Email = email;
            SetPassword(rawPassword); // Hash the password upon creation
        }

        public void SetPassword(String rawPassword) {
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public bool CheckPassword(String rawPassword) {
            return BCrypt.Net.BCrypt.Verify(rawPassword, HashedPassword);
        }
    }
}