namespace UnifiedLearningApi.Models
{
    /// <summary>
    /// COMMENT ENTITY - Cấu trúc cha/con nhiều cấp.
    /// Đây chính là cấu trúc có thể gây vòng lặp vô hạn.
    /// </summary>
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";
        // Liên kết với post
        public int PostId { get; set; }
        public Post? Post { get; set; }
        // Comment cha
        public int? ParentId { get; set; }
        public Comment? parent { get; set; }
        // Danh sách comment con
        public List<Comment> Children { get; set; } = new();
    }
}
