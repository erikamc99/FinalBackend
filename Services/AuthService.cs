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

        public async Task<User> GetProfile(string userId)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(userId);
            var user = await _context.Users.Find(u => u.Id == objectId).FirstOrDefaultAsync();
            if (user == null) throw new Exception("Usuario no encontrado");
            return user;
        }

        public async Task<User> UpdateProfile(string userId, UpdateProfileDto dto)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(userId);
            var update = Builders<User>.Update
                .Set(u => u.AvatarUrl, dto.AvatarUrl)
                .Set(u => u.Name, dto.Name)
                .Set(u => u.Username, dto.Username)
                .Set(u => u.Email, dto.Email);

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                update = update.Set(u => u.PasswordHash, passwordHash);
            }

            var result = await _context.Users.UpdateOneAsync(
                u => u.Id == objectId,
                update);

            if (result.MatchedCount == 0) throw new Exception("Usuario no encontrado");

            return await GetProfile(userId);
        }
    }
}