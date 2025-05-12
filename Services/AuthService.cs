using Apilogin.Models;
using Apilogin.Dtos;
using Apilogin.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Apilogin.Services
{
    public class AuthService
    {
        private readonly InMemoryDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(InMemoryDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public User Register(RegisterDto registerDto)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == registerDto.Username);
            if (existingUser != null)
            {
                return null; // User already exists
            }

            var user = new User
            {
                Id = _context.Users.Count + 1,
                Username = registerDto.Username,
                Password = registerDto.Password // In production, hash this!
            };

            _context.Users.Add(user);
            return user;
        }

        public string? Login(LoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username && u.Password == loginDto.Password);
            if (user == null) return null;

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]?? throw new Exception("JWT Key not found in configuration"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiresInMinutes"] ?? throw new Exception("ExpiryMinutes not configured"))),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void Logout()
        {
            // No server-side logic for JWT logout unless using blacklists or token revocation
        }
    }
}
