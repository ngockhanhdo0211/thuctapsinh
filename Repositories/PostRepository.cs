using Microsoft.EntityFrameworkCore;
using UnifiedLearningApi.Data;
using UnifiedLearningApi.Models;
using UnifiedLearningApi.Repositories.Interfaces;

namespace UnifiedLearningApi.Repositories
{
    /// <summary>
    /// Repository cho Post – chỉ xử lý truy vấn database.
    /// Không chứa logic nghiệp vụ.
    /// </summary>
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _db;

        public PostRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _db.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _db.Posts.FindAsync(id);
        }

        public async Task<Post> AddAsync(Post post)
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return false;

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
