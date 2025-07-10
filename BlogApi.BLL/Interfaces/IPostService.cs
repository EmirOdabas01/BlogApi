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
        Task<List<Post>> GetAllPosts();
        Task<Post?> GetPostById(int id);
        Task<OperationResult> AddPost(Post entity);
        Task<OperationResult> UpdatePost(Post entity);
        Task<OperationResult> RemovePost(Post entity);
    }
}
