using BlogApi.BLL.Dtos;
using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>> GetAllPostByCategoryAsync(PostType category);
        Task<Post?> GetPostByCategoryAsync(PostType category);
        Task<Post?> GetPostByIdAsync(int id);
        Task<OperationResult> AddPostAsync(PostDto entity);
        Task<OperationResult> UpdatePostAsync(PostDto entity);
        Task<OperationResult> RemovePostAsync(int id);
    }
}
