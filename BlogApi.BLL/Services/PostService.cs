using BlogApi.BLL.Interfaces;
using BlogApi.DAL.Interfaces;
using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            OperationResult Result = new();

            if(entity.Blocks.Any(u =>u.PostId != entity.Id))
            {
                Result.Message = "post block incompatibility";
                _logger.LogWarning(Result.Message);
                return Result;
            }

            if (entity.Blocks.Any(b => !Enum.IsDefined(typeof(BlockType), b.BlockCategory)))
            {
                Result.Message = "invalid block category";
                _logger.LogWarning(Result.Message);
                return Result;
            }


            var orderedBlocks = entity.Blocks.Select(p => p.Order).ToList();
            int min = orderedBlocks.Min();
            int max = orderedBlocks.Max();

            Result.Success = min == 0 && orderedBlocks.Count == (max - min + 1);
            if(!Result.Success)
            {
                Result.Message = "Wrong order ranking";
                _logger.LogWarning(Result.Message);
                return Result;
            }

            try
            {
                Result.Success = await _postRepo.AddPost(entity);

                if (Result.Success)
                {
                    Result.Message = $"New post is created with header {entity.Header}";
                    _logger.LogInformation(Result.Message);
                }
                else
                {
                    Result.Message = "Cannot created new post";
                    _logger.LogWarning(Result.Message);
                }
            }
            catch (Exception ex)
            {
                Result.Message = "An exception thrown from db";
                _logger.LogError(ex, Result.Message);
            }
            return Result;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            try
            {
                var posts = await _postRepo.GetAllPosts();

                if(posts is not null)
                {
                    _logger.LogInformation("GetAllPosts is done");
                    return posts;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception thrown from db");
            }
                return null;
        }

        public async Task<Post?> GetPostById(int id)
        {
            if (id < 0)
            {
                _logger.LogWarning("invalid id value");
                return null;
            }
            try
            {
                var post = await _postRepo.GetPostById(id);
                
                if(post is not null)
                {
                    _logger.LogInformation($"GetPostBy id is success for post with id : {post.Id}");
                    return post;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An exception thrown from db");
            }
            return null;
        }

        public async Task<OperationResult> RemovePost(Post entity)
        {
            OperationResult Result = new();
            try
            {
                Result.Success = await _postRepo.RemovePost(entity);

                if (Result.Success)
                {
                    Result.Message = $"Removing post is done for post id with {entity.Id}";
                    _logger.LogInformation(Result.Message);
                }
                else
                {
                    Result.Message = $"Cannot remove the post with id {entity.Id}";
                    _logger.LogWarning(Result.Message);
                }
            }
            catch (Exception ex)
            {
                Result.Message = "An exception thrown from db";
                _logger.LogError(ex, Result.Message);
            }
            return Result;
        }

        public async Task<OperationResult> UpdatePost(Post entity)
        {
            OperationResult Result = new();
            try
            {
                Result.Success = await _postRepo.UpdatePost(entity);

                if (Result.Success)
                {
                    Result.Message = $"Updating post is done for post id with {entity.Id}";
                    _logger.LogInformation(Result.Message);
                }
                else
                {
                    Result.Message = $"Cannot update the post with id {entity.Id}";
                    _logger.LogWarning(Result.Message);
                }
            }
            catch (Exception ex)
            {
                Result.Message = "An exception thrown from db";
                _logger.LogError(ex, Result.Message);
            }
            return Result;
        }
    }
}
