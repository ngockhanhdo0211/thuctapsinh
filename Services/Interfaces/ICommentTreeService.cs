using UnifiedLearningApi.DTOs;

namespace UnifiedLearningApi.Services.Interfaces
{
    /// <summary>
    /// SERVICE LAYER INTERFACE dành riêng cho Comment Tree (đa cấp).
    /// Dùng đệ quy để build dạng cây.
    /// </summary>
    public interface ICommentTreeService
    {
        Task<List<CommentDto>> GetTreeByPostAsync(int postId);
    }
}
