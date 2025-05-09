using Apilogin.Models;
using Apilogin.Dtos;
using Apilogin.Data;
using System.Linq;

namespace Apilogin.Services
{
    public class AuthService
    {
        private readonly InMemoryDbContext _context;

        public AuthService(InMemoryDbContext context)
        {
            _context = context;
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
                Password = registerDto.Password // Use hashing in real apps
            };

            _context.Users.Add(user);
            return user;
        }

        public User Login(LoginDto loginDto)
        {
            return _context.Users.FirstOrDefault(u => u.Username == loginDto.Username && u.Password == loginDto.Password);
        }

        public void Logout() { }
    }
}
