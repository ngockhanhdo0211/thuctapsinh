namespace UnifiedLearningApi.DTOs
{
    /// <summary>
    /// DTO trả về bài viết đơn giản, không trả comments ở đây.
    /// </summary>
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
    }
}
