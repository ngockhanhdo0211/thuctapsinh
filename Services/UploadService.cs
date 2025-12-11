using UnifiedLearningApi.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace UnifiedLearningApi.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _env;

        public UploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // ================================================================
        // Save file (avatar / attachment) với thư mục được truyền vào
        // ================================================================
        public string SaveFile(IFormFile file, string folder)
        {
            // Tạo thư mục nếu chưa có
            string folderPath = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Validate extension hợp lệ
            string ext = Path.GetExtension(file.FileName).ToLower();
            string[] allowed = { ".jpg", ".png", ".jpeg", ".gif", ".pdf", ".docx" };

            if (!allowed.Contains(ext))
                throw new Exception("File không hợp lệ!");

            // Tên file mới
            string newFile = Guid.NewGuid() + ext;
            string fullPath = Path.Combine(folderPath, newFile);

            // Lưu file vào server
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Trả URL public cho client
            return $"/{folder}/{newFile}";
        }
    }
}
