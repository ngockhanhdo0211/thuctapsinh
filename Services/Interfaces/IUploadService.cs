using Microsoft.AspNetCore.Http;

namespace UnifiedLearningApi.Services.Interfaces
{
    public interface IUploadService
    {
        /// <summary>
        /// Lưu file vào thư mục chỉ định và trả về tên file mới.
        /// </summary>
        /// <param name="file">File upload từ client</param>
        /// <param name="folder">Tên thư mục (ví dụ: "avatars", "attachments")</param>
        /// <returns>Tên file mới sau khi lưu</returns>
        string SaveFile(IFormFile file, string folder);
    }
}
