using BlogApi.BLL.Interfaces;
using BlogApi.DAL.Interfaces;
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

        public async Task<bool> AddPost(Post entity)
        {
            bool success = false;
            try
            {
                success = await _postRepo.AddPost(entity);

                if (success)
                    _logger.LogInformation($"New post is created with header {entity.Header}");
                else
                    _logger.LogWarning("Cannot created new post");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception thrown from db");
            }
            return success;
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

        public async Task<bool> RemovePost(Post entity)
        {
            bool success = false;
            try
            {
                success = await _postRepo.RemovePost(entity);

                if (success)
                    _logger.LogInformation($"Removing post is done for post id with {entity.Id}");
                else
                    _logger.LogWarning($"Cannot remove the post with id {entity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception thrown from db");
            }
            return success;
        }

        public async Task<bool> UpdatePost(Post entity)
        {
            bool success = false;
            try
            {
                success = await _postRepo.UpdatePost(entity);

                if (success)
                    _logger.LogInformation($"Updating post is done for post id with {entity.Id}");
                else
                    _logger.LogWarning($"Cannot update the post with id {entity.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception thrown from db");
            }
            return success;
        }
    }
}
