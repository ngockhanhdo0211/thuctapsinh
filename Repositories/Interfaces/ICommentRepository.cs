using UnifiedLearningApi.Models;

namespace UnifiedLearningApi.Repositories.Interfaces
{
    /// <summary>
    /// REPOSITORY INTERFACE cho Comment.
    /// </summary>
    public interface ICommentRepository
    {
        Task<List<Comment>> GetByPostIdAsync(int postId);
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> AddAsync(Comment comment);
        Task<bool> DeleteAsync(int id);
    }
}
