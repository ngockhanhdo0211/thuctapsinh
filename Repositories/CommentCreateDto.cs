using Microsoft.EntityFrameworkCore;
using UnifiedLearningApi.Data;
using UnifiedLearningApi.Models;
using UnifiedLearningApi.Repositories.Interfaces;

namespace UnifiedLearningApi.Repositories
{
    /// <summary>
    /// Repository cho Comment – hỗ trợ CRUD & lấy comment theo PostId.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _db;

        public CommentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Comment>> GetByPostIdAsync(int postId)
        {
            return await _db.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _db.Comments.FindAsync(id);
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment == null) return false;

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
