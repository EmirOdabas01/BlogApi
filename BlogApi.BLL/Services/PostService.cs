using BlogApi.BLL.Dtos;
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

        public async Task<OperationResult> AddPostAsync(PostDto entity)
        {
            OperationResult result = new();

            Post post = new();
            FromDtoToPost(entity, post);

            result = PostValidation(post);
            if (!result.Success) return result;

            result.Success = await _postRepo.AddPostAsync(post);
            result.Message = result.Success
                ? $"New post is created with header: {entity.Header}"
                : "Cannot create new post";

            _logger.Log(result.Success ? LogLevel.Information : LogLevel.Warning, result.Message);

            return result;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            var posts = await _postRepo.GetAllPostsAsync();
            _logger.LogInformation("GetAllPosts completed");
            return posts;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            if (id < 0)
            {
                _logger.LogWarning("Invalid post ID");
                return null;
            }

            var post = await _postRepo.GetPostByIdAsync(id);
            if (post != null)
                _logger.LogInformation($"Post fetched successfully with ID: {post.Id}");

            return post;
        }

        private OperationResult PostValidation(Post entity)
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

        public async Task<OperationResult> RemovePostAsync(int id)
        {
            OperationResult result = new();

            result.Success = await _postRepo.RemovePostAsync(id);
            result.Message = result.Success
                ? $"Post removed with ID: {id}"
                : $"Failed to remove post with ID: {id}";

            _logger.Log(result.Success ? LogLevel.Information : LogLevel.Warning, result.Message);

            return result;
        }

        public async Task<OperationResult> UpdatePostAsync(PostDto entity)
        {
            OperationResult result = new();


            var post = await _postRepo.GetPostByIdAsync(Convert.ToInt32(entity.Id));
            if (post == null)
            {
                result.Message = "Post not found";
                return result;
            }

            post.Blocks.Clear();

            FromDtoToPost(entity, post);

            result = PostValidation(post);
            if (!result.Success) return result;

            result.Success = await _postRepo.UpdatePostAsync(post);
            result.Message = result.Success
                ? $"Post updated with ID: {entity.Id}"
                : $"Failed to update post with ID: {entity.Id}";

            _logger.Log(result.Success ? LogLevel.Information : LogLevel.Warning, result.Message);

            return result;
        }

        private void FromDtoToPost(PostDto entity, Post post)
        {
            post.Header = entity.Header;
            post.PostCategory = entity.PostCategory;


            foreach (var blockDto in entity.Blocks)
            {
                var newBlock = new PostBlock
                {
                    BlockCategory = blockDto.BlockCategory,
                    Content = blockDto.Content,
                    ImageUrl = blockDto.ImageUrl,
                    Order = blockDto.Order,
                    PostId = post.Id
                };

                post.Blocks.Add(newBlock);
            }
        }

        public async Task<List<Post>> GetAllPostByCategoryAsync(PostType category)
        {
            if (!Enum.IsDefined(typeof(PostType), category)) return new();
            return await _postRepo.GetAllByCategoryAsync(p => p.PostCategory == category);
        }

        public async Task<Post?> GetPostByCategoryAsync(PostType category)
        {
            if (!Enum.IsDefined(typeof(PostType), category)) return new();
            return await _postRepo.GetPostByCategoryAsync(category);
        }
    }
}
