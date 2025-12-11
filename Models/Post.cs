namespace UnifiedLearningApi.Models
{
    /// <summary>
    /// POST ENTITY
    /// Dây là bảng bài viết chính
    /// </summary>
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        // Navigation: Một bài viết có nhiều comment
        public List<Comment> Comments { get; set; } = new();
    }
}
