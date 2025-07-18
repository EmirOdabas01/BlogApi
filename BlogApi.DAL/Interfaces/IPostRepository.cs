using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>> GetAllByCategoryAsync(Expression<Func<Post, bool>> expression);
        Task<Post?> GetPostByIdAsync(int id);
        Task<bool> AddPostAsync(Post entity);
        Task<bool> UpdatePostAsync(Post entity);
        Task<bool> RemovePostAsync(int id);
        Task<bool> SaveDbAsync();
    }
}
