using BlogApi.BLL.Dtos;
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
        Task<Post?> GetPostByIdAsync(int id);
        Task<OperationResult> AddPostAsync(Post entity);
        Task<OperationResult> UpdatePostAsync(Post entity);
        Task<OperationResult> RemovePostAsync(int id);
        OperationResult PostValidation(Post entity);
    }
}
