namespace UnifiedLearningApi.DTOs
{
    public class CommentCreateDto
    {
        public string Content { get; set; } = "";
        public int PostId { get; set; }
        public int? ParentId { get; set; }
    }
}
