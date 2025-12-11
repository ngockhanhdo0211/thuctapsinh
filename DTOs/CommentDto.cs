namespace UnifiedLearningApi.DTOs
{
    /// <summary>
    /// DTO dạng cây để trả ra API comment tree tránh vòng lặp vô hạn.
    /// </summary>
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public List<CommentDto> Children { get; set; } = new();
    }
}
