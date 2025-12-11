using UnifiedLearningApi.Models;

namespace UnifiedLearningApi.Repositories.Interfaces
{
    /// <summary>
    /// REPOSITORY INTERFACE quản lý CRUD cho Post.
    /// </summary>
    public interface IPostRepository
    {
        Task<List<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int id);
        Task<Post> AddAsync(Post post);
        Task<bool> DeleteAsync(int id);
    }
}
