using Muuki.Models;
using Muuki.DTOs;
using Muuki.Data;
using Muuki.Utils;
using MongoDB.Driver;

namespace Muuki.Services
{
    public class AuthService
    {
        private readonly MongoContext _context;
        private readonly JwtUtils _jwt;

        public AuthService(MongoContext context, JwtUtils jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var exists = await _context.Users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (exists != null) throw new Exception("Usuario ya registrado");

            var user = new User
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _context.Users.InsertOneAsync(user);
            return _jwt.GenerateToken(user);
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Credenciales inválidas");

            return _jwt.GenerateToken(user);
        }
    }
}