using UnifiedLearningApi.Data;
using UnifiedLearningApi.DTOs;
using UnifiedLearningApi.Models;
using UnifiedLearningApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UnifiedLearningApi.Services
{
    /// <summary>
    /// Service build comment tree (cha – con – cháu).
    /// Tránh vòng lặp vô hạn bằng DTO dạng cây.
    /// </summary>
    public class CommentTreeService : ICommentTreeService
    {
        private readonly AppDbContext _db;

        public CommentTreeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CommentDto>> GetTreeByPostAsync(int postId)
        {
            // Lấy toàn bộ comment dạng phẳng theo PostId
            var list = await _db.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();

            // Build tree bắt đầu từ comment gốc
            return BuildTree(list, null);
        }

        /// <summary>
        /// Hàm đệ quy để build cây comment nhiều cấp.
        /// </summary>
        private List<CommentDto> BuildTree(List<Comment> comments, int? parentId)
        {
            return comments
                .Where(c => c.ParentId == parentId)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    Children = BuildTree(comments, c.Id)   // Đệ quy
                })
                .ToList();
        }
    }
}
