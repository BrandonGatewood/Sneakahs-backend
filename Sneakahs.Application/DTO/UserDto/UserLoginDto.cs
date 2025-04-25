using System;

namespace Sneakahs.Application.DTO.UserDto
{
    public class UserLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}