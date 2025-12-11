namespace UnifiedLearningApi.Models
{
    public class User
    {
        public int Id { get; set; }
        // Email dùng để đăng nhập
        public string Email { get; set; } = null!;
        // Mật khẩu đã được hash
        public string PasswordHash { get; set; } = null!;
        //Role: Admin hoặc User
        public string Role { get; set; } = "User";
        // Một User có thể có nhiều RefreshToken
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
