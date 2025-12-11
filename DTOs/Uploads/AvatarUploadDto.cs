using Microsoft.AspNetCore.Http;
namespace UnifiedLearningApi.DTOs.Upload
{
    public class AvatarUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
