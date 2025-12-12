using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UnifiedLearningApi.DTOs;
using UnifiedLearningApi.Services.Interfaces;

namespace UnifiedLearningApi.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentTreeService _treeService;

        public CommentsController(ICommentTreeService treeService)
        {
            _treeService = treeService;
        }

        [HttpGet("tree/{postId}")]
        public async Task<IActionResult> GetTree(int postId)
        {
            var result = await _treeService.GetTreeByPostAsync(postId);
            return Ok(result);
        }
    }
}
