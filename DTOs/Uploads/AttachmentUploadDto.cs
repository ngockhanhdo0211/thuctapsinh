namespace UnifiedLearningApi.DTOs.Uploads
{
    public class AttachmentUploadDto
    {
        public IFormFile File { get; set; } = null!;
        public int PostId { get; set; }
    }
}
