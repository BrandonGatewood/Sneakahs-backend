using System;

namespace Sneakahs.Application.DTO.UserDto
{
    public class UserLoginDto
    {
        public required String Email { get; set; }
        public required String Password { get; set; }

    }
}