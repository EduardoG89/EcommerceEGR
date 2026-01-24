using Ecommerce.Data;
using Ecommerce.DTOs.Auth;
using Ecommerce.Models;
using Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;
        private string token = "";

        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                throw new Exception("Credenciales invalidas");

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                throw new Exception("Credenciales invalidas");

            token = GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                Token = token,
            };

        }

        private string GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.Email))
                throw new Exception("El email ya esta en uso");

            var user = new User
            {
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                IsAdmin = false,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            token = GenerateToken(user);

            return new AuthResponseDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                Token = token
            };


        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
