namespace Sneakahs.Application.DTO.AuthDto
{
    public class UserLoginDto
    { 
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}