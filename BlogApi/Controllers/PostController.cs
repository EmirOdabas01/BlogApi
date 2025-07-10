using BlogApi.BLL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    public class PostController : Controller
    {
        [HttpGet("get-post/{id}")]
        public Task<IActionResult> GetPostById(int id)
        {
            return null;
        }

        [HttpGet("get-all")]
        public Task<IActionResult> GetAllPosts()
        {

            return null;
        }

        [HttpPost("create-post")]
        public Task<IActionResult> CreatePost([FromBody] PostDto entity)
        {
            return null;

        }
    }
}
