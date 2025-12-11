using Microsoft.EntityFrameworkCore;
using UnifiedLearningApi.Models;

namespace UnifiedLearningApi.Data
{
    /// <summary>
    /// DbContext chính – bao gồm cả Post và Comment.
    /// Dùng InMemoryDatabase cho dễ test trong học kỳ.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<User> User { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
