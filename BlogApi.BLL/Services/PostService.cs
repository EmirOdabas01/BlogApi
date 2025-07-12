using BlogApi.BLL.Interfaces;
using BlogApi.DAL.Interfaces;
using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using Microsoft.Extensions.Logging;

namespace BlogApi.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly ILogger<PostService> _logger;

        public PostService(IPostRepository postRepo, ILogger<PostService> logger)
        {
            _postRepo = postRepo;
            _logger = logger;
        }

        public async Task<OperationResult> AddPost(Post entity)
        {
            OperationResult result = new();

            result = PostValidation(entity);
            if (!result.Success) return result;

            result.Success = await _postRepo.AddPost(entity);
            result.Message = result.Success
                ? $"New post is created with header: {entity.Header}"
                : "Cannot create new post";

            _logger.Log(result.Success ? LogLevel.Information : LogLevel.Warning, result.Message);

            return result;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await _postRepo.GetAllPosts();
            _logger.LogInformation("GetAllPosts completed");
            return posts;
        }

        public async Task<Post?> GetPostById(int id)
        {
            if (id < 0)
            {
                _logger.LogWarning("Invalid post ID");
                return null;
            }

            var post = await _postRepo.GetPostById(id);
            if (post != null)
                _logger.LogInformation($"Post fetched successfully with ID: {post.Id}");

            return post;
        }

        public OperationResult PostValidation(Post entity)
        {
            OperationResult result = new();

            if (!Enum.IsDefined(typeof(PostType), entity.PostCategory))
            {
                result.Message = "Invalid post category";
                _logger.LogWarning(result.Message);
                return result;
            }

            if (entity.Blocks.Any(b => !Enum.IsDefined(typeof(BlockType), b.BlockCategory)))
            {
                result.Message = "Invalid block category";
                _logger.LogWarning(result.Message);
                return result;
            }

            if (entity.Blocks.Any(b => b.PostId != entity.Id))
            {
                result.Message = "Post-block incompatibility";
                _logger.LogWarning(result.Message);
                return result;
            }
            //checking order ranking
            var orderList = entity.Blocks.Select(b => b.Order).ToList();
            int min = orderList.Min();
            int max = orderList.Max();

            result.Success = min == 0 && orderList.Count == (max - min + 1);
            if (!result.Success)
            {
                result.Message = "Wrong block order ranking";
                _logger.LogWarning(result.Message);
                return result;
            }

            return result;
        }

        public async Task<OperationResult> RemovePost(int id)
        {
            OperationResult result = new();

            result.Success = await _postRepo.RemovePost(id);
            result.Message = result.Success
                ? $"Post removed with ID: {id}"
                : $"Failed to remove post with ID: {id}";

            _logger.Log(result.Success ? LogLevel.Information : LogLevel.Warning, result.Message);

            return result;
        }

        public async Task<OperationResult> UpdatePost(Post entity)
        {
            OperationResult result = new();

            result = PostValidation(entity);
            if (!result.Success) return result;

            result.Success = await _postRepo.UpdatePost(entity);
            result.Message = result.Success
                ? $"Post updated with ID: {entity.Id}"
                : $"Failed to update post with ID: {entity.Id}";

            _logger.Log(result.Success ? LogLevel.Information : LogLevel.Warning, result.Message);

            return result;
        }
    }
}
