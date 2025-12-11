using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UnifiedLearningApi.DTOs;
using UnifiedLearningApi.Services.Interfaces;

namespace UnifiedLearningApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
