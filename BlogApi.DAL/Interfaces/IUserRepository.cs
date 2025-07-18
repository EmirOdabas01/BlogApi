using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username);
        Task<User?> GetUserAsync(int id);
        Task SaveDbForRefreshTokenAsync();
    }
}
