using BlogApi.DAL.Interfaces;
using BlogApi.Entities.Enums;
using BlogApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<bool> AddPostAsync(Post entity)
        {
            await _postDbSet.AddAsync(entity);
            return await SaveDbAsync();
        }

        public async Task<List<Post>> GetAllByCategoryAsync(Expression<Func<Post, bool>> expression)
        {
            return await _postDbSet.Include(p => p.Blocks).Where(expression).ToListAsync();
        }
        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _postDbSet.Include(p => p.Blocks).
                Where(p => p.PostCategory == PostType.Technology || p.PostCategory == PostType.Blog)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByCategoryAsync(PostType category)
        {
            return await _postDbSet.Include(p => p.Blocks).FirstOrDefaultAsync(post => post.PostCategory == category);
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _postDbSet.Include(P => P.Blocks).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> RemovePostAsync(int id)
        {
            var post = await _postDbSet.FindAsync(id);
            if (post is null) return false;

            _postDbSet.Remove(post);
            return await SaveDbAsync();
        }

        public async Task<bool> SaveDbAsync()
        {
            return await _blogApiContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePostAsync(Post entity)
        {
            _postDbSet.Update(entity);
            return await SaveDbAsync();
        }
    }
}
