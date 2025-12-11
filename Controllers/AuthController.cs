using Microsoft.AspNetCore.Mvc;
using UnifiedLearningApi.DTOs.Auth;
using UnifiedLearningApi.Services.Interfaces;

namespace UnifiedLearningApi.Controllers
{
    // Đánh dấu đây là API Controller
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        // Inject AuthService để sử lý logic đăng ký, đăng nhập, refresh token

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }
        // 1) API Đăng Ký

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var success = await _auth.RegisterAsync(dto);
            return success ? Ok("Registered!") : BadRequest("Email already exists.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _auth.LoginAsync(dto);
            return result == null ? Unauthorized() : Ok(result);
        }
        // Kiểm tra refresh token có hợp lệ + còn hạn hay không

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(string token)
        {
            // 401 - Token hết hạn hoặc sai
            // 200 - Trả về AccessToken mới
            var result = await _auth.RefreshTokenAsync(token);
            return result == null ? Unauthorized() : Ok(result);
        }
    }
}
