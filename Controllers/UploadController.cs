using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UnifiedLearningApi.DTOs.Upload;
using UnifiedLearningApi.Services.Interfaces;
using UnifiedLearningApi.DTOs.Uploads; // ← cái này nếu không xài thì xóa
namespace UnifiedLearningApi.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")] 
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _upload;

        public UploadController(IUploadService upload)
        {
            _upload = upload;
        }

        [HttpPost("avatar")]
        public IActionResult UploadAvatar([FromForm] AvatarUploadDto dto)
        {
            if (dto.File == null)
                return BadRequest("Không có file!");

            var url = _upload.SaveFile(dto.File, "avatars");
            return Ok(new { avatarUrl = url });
        }

        [HttpPost("post-attachment")]
        public IActionResult UploadAttachment([FromForm] AttachmentUploadDto dto)
        {
            if (dto.File == null)
                return BadRequest("Không có file!");

            var url = _upload.SaveFile(dto.File, "attachments");
            return Ok(new { fileUrl = url });
        }
    }
}
