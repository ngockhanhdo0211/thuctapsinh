using AutoMapper;
using UnifiedLearningApi.DTOs;
using UnifiedLearningApi.Models;
using UnifiedLearningApi.Repositories.Interfaces;
using UnifiedLearningApi.Services.Interfaces;

namespace UnifiedLearningApi.Services
{
    /// <summary>
    /// Service xử lý logic nghiệp vụ cho Post.
    /// Không truy cập DB trực tiếp, chỉ gọi Repository.
    /// </summary>
    public class PostService : IPostService
    {
        private readonly IPostRepository _repo;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> GetPostsAsync()
        {
            var posts = await _repo.GetAllAsync();
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<PostDto?> GetPostAsync(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> CreateAsync(PostCreateDto dto)
        {
            var entity = _mapper.Map<Post>(dto);
            var saved = await _repo.AddAsync(entity);
            return _mapper.Map<PostDto>(saved);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}
