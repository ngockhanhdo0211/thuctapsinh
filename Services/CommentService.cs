using AutoMapper;
using UnifiedLearningApi.DTOs;
using UnifiedLearningApi.Models;
using UnifiedLearningApi.Repositories.Interfaces;
using UnifiedLearningApi.Services.Interfaces;

namespace UnifiedLearningApi.Services
{
    /// <summary>
    /// Service xử lý CRUD comment đơn giản.
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repo;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CommentDto?> GetAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> CreateAsync(CommentCreateDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            var saved = await _repo.AddAsync(entity);
            return _mapper.Map<CommentDto>(saved);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}
