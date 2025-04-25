using System;

namespace Sneakahs.Application.DTO.UserDto
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
    }
}