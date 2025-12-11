using AutoMapper;
using UnifiedLearningApi.DTOs;
using UnifiedLearningApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnifiedLearningApi.Mappings
{
    /// <summary>
    /// AutoMapper Profile: định nghĩa map Entity ↔ DTO.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // POST
            CreateMap<Post, PostDto>();
            CreateMap<PostCreateDto, Post>();

            // COMMENT
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentCreateDto, Comment>();
        }
    }
}
