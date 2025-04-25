using System;

namespace Sneakahs.Application.DTO.UserDto
{
    public class UserResponseDto
    {
        public required Guid Id { get; set; }
        public required string token { get; set; }
    }
}