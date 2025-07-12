using BlogApi.DAL.Interfaces;
using BlogApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogApiContext _blogApiContext;
        private readonly DbSet<Post> _postDbSet;

        public PostRepository(BlogApiContext blogApiContext)
        {
            _blogApiContext = blogApiContext;
            _postDbSet = _blogApiContext.Set<Post>();
        }

        public async Task<bool> AddPost(Post entity)
        {
            await _postDbSet.AddAsync(entity);
            return await SaveDb();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _postDbSet.Include(p => p.Blocks).ToListAsync();
        }

        public async Task<Post?> GetPostById(int id)
        {
            return await _postDbSet.FindAsync(id);
        }

        public async Task<bool> RemovePost(Post entity)
        {
            _postDbSet.Remove(entity);
            return await SaveDb();
        }

        public async Task<bool> SaveDb()
        {
            return await _blogApiContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePost(Post entity)
        {
            _postDbSet.Update(entity);
            return await SaveDb();
        }
    }
}
