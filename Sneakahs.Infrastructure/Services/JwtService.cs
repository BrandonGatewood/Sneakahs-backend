using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sneakahs.Application.Interfaces.Services;
using Sneakahs.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// JWT Creation, Token Validation, JWT secret handling.
namespace Sneakahs.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly string _issuer; 
        private readonly string _audience;

        // Constructor to inject Iconfiguration
        public JwtService(string secret, string issuer, string audience)
        {
            _secret = secret;
            _issuer = issuer;
            _audience = audience;
        }

        // Generate JWT for user authentication
        public string GenerateToken(User user) {
            // Claims based on user data
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Get the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate the token
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            // Return token as string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? validateToken(string token)
        {
            try
            {
                // Get the secret from the configuration (generated dynamically in Program.cs)
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

                // Validate token parameters
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = key
                };

                // Validate the token and get claims principal (user identity)
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                return principal;
            }
            catch (Exception)
            {
                // In case of failure, return null (could be customized to throw an exception)
                return null;
            }
        }
        public string generateRefreshToken() {
            var randomBytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}