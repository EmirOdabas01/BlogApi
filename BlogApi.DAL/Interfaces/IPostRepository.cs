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
        Task<List<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(int id);
        Task<bool> AddPostAsync(Post entity);
        Task<bool> UpdatePostAsync(Post entity);
        Task<bool> RemovePostAsync(int id);
        Task<bool> SaveDbAsync();
    }
}
