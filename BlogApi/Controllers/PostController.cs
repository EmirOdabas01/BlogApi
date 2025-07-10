using BlogApi.BLL.Dtos;
using BlogApi.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet("get-post/{id}")]
        public Task<IActionResult> GetPostById(int id)
        {
            return null;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPosts()
        {
            _logger.LogInformation("GetAllPosts is called");
            var result = await _postService.GetAllPosts();

            if (result is not null) return Ok(result);

            return NotFound("There is no existing post");
        }

        [HttpPost("create-post")]
        public Task<IActionResult> CreatePost([FromBody] PostDto entity)
        {
            return null;

        }
    }
}
