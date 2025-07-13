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
    public class UserRepository : IUserRepository
    {
        private readonly BlogApiContext _blogApiContext;

        public UserRepository(BlogApiContext blogApiContext)
        {
            _blogApiContext = blogApiContext;
        }

        public async Task<User?> GetUser(string username)
        {
            return await _blogApiContext.Set<User>().FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task<User?> GetUser(int id)
        {
            return await _blogApiContext.Set<User>().FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task SaveDbForRefreshToken()
        {
             await _blogApiContext.SaveChangesAsync();
        }
    }
}
