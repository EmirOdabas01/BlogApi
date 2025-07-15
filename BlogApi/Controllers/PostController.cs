using BlogApi.BLL.Dtos;
using BlogApi.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BlogApi.Controllers
{
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetPostByIdAsync(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return post is null
                ? NotFound("This post is not exist")
                : Ok(post);
        }
   
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            var posts = await _postService.GetAllPostsAsync();

            return posts is null || !posts.Any()
                ? NotFound("There is no post exist")
                : Ok(posts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePostAsync([FromBody] PostDto entity)
        {
            if (entity == null) return BadRequest("Not valid instance");

            var result = await _postService.AddPostAsync(entity);

            return result.Success
                ? Ok("New post is created")
                : BadRequest(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePostAsync([FromBody] PostDto entity)
        {
            if (entity == null || entity.Id == null)
                return BadRequest("Not valid instance");

            var result = await _postService.UpdatePostAsync(entity);

            return result.Success
                ? Ok("Successfully updated")
                : BadRequest(result.Message);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            if (id < 0) return BadRequest("Invalid id");

            var result = await _postService.RemovePostAsync(id);

            return result.Success
                ? Ok("Successfully deleted")
                : BadRequest(result.Message);
        }
    }
}
