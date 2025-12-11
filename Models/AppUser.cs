using System;
using System.Collections.Generic;

namespace UnifiedLearningApi.Models
{
    public class AppUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        // Navigation: 1 User có nhiều RefreshTokens
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
