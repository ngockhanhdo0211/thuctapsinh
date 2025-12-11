using UnifiedLearningApi.DTOs;

namespace UnifiedLearningApi.Services.Interfaces
{
    /// <summary>
    /// SERVICE LAYER INTERFACE cho Post.
    /// Chỉ định nghĩa chức năng mà Service cần triển khai.
    /// </summary>
    public interface IPostService
    {
        Task<List<PostDto>> GetPostsAsync();
        Task<PostDto?> GetPostAsync(int id);
        Task<PostDto> CreateAsync(PostCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
