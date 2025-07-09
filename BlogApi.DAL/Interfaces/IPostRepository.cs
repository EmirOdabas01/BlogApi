using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPosts();
        Task<Post?> GetPostById(int id);
        Task<bool> AddPost(Post entity);
        Task<bool> UpdatePost(Post entity);
        Task<bool> RemovePost(Post entity);
        Task<bool> SaveDb();
    }
}
