using UnifiedLearningApi.DTOs;

namespace UnifiedLearningApi.Services.Interfaces
{
    /// <summary>
    /// SERVICE LAYER INTERFACE dành cho Comment CRUD.
    /// </summary>
    public interface ICommentService
    {
        Task<CommentDto?> GetAsync(int id);
        Task<CommentDto> CreateAsync(CommentCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
