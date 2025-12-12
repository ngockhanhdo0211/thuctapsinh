using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UnifiedLearningApi.Data;
using UnifiedLearningApi.DTOs.Auth;
using UnifiedLearningApi.Models;
using UnifiedLearningApi.Services.Interfaces;

namespace UnifiedLearningApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // REGISTER
        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            // Kiểm tra email đã tồn tại trong database hay chưa
            if (await _db.User.AnyAsync(u => u.Email == dto.Email))
                return false;
                // Mật khẩu bằng BCrypt để bảo mật

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // Tạo user mới

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = hash, // lưu thành hash, không lưu mật khẩu chính
                Role = dto.Role
            };
            // Thêm user vào database 

            _db.User.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        // LOGIN
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // Lấy user theo email, kèm refresh tokens
            var user = await _db.User
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null) return null;
            // Kiểm tra mật khẩu có đúng không bằng BCrypt

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;
                // trả về access token + refresh token

            return await GenerateTokensAsync(user);
        }

        // REFRESH TOKEN
        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            // tìm refresh token hợp lệ trong database
            var token = await _db.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r =>
                    r.Token == refreshToken &&
                    !r.IsRevoked &&
                    r.ExpiresAt > DateTime.UtcNow);

            if (token == null) return null;

            return await GenerateTokensAsync(token.User);
        }
        // Tạo Access & Refresh token

        private async Task<AuthResponseDto> GenerateTokensAsync(User user)
        {
            var access = GenerateJwtToken(user);
            var refresh = GenerateRefreshToken();

            var rt = new RefreshToken
            {
                Token = refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = access,
                RefreshToken = refresh
            };
        }
        // Tạo JWWT TOKEN

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Thông tin nhúng trong token

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            // Tạo JWWT token

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20), // Thời hạn access token trong vòng 20p
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
