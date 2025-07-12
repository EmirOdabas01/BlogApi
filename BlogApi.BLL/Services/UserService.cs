using BlogApi.BLL.Dtos;
using BlogApi.BLL.Interfaces;
using BlogApi.DAL.Interfaces;
using BlogApi.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepo, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<User?> GetIfIsAdmin(UserDto entity)
        {
            
            var user = await _userRepo.GetUser(entity.UserName);

            if (user is null)
            {
                _logger.LogWarning("user is not exist");
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, entity.Password) == PasswordVerificationResult.Failed) return null;
            _logger.LogInformation("User is authenticated");
            return user;
        }
    }
}
