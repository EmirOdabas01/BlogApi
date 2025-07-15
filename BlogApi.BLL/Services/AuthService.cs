using BlogApi.BLL.Dtos;
using BlogApi.BLL.Interfaces;
using BlogApi.DAL.Interfaces;
using BlogApi.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlogApi;
namespace BlogApi.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepo, ILogger<AuthService> logger, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<User?> GetIfIsAdminAsync(UserDto entity)
        {
            
            var user = await _userRepo.GetUserAsync(entity.UserName);

            if (user is null)
            {
                _logger.LogWarning("user is not exist");
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, entity.Password) == PasswordVerificationResult.Failed) return null;
            _logger.LogInformation("User is authenticated");
            return user;
        }
        public async Task<TokenResponseDto?> GenerateTokensAsync(User user)
        {
            TokenResponseDto token = new()
            {
                AccessToken = CreateAccessToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };

            return token;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null) return null;

            return await GenerateTokensAsync(user);
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepo.GetUserAsync(userId);
            if (user is null || user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;
            return user;
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepo.SaveDbForRefreshTokenAsync();
            return refreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)
                );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokendescriptor = new JwtSecurityToken(
               issuer: _configuration.GetSection("AppSettings:Issuer").Value,
               audience: _configuration.GetSection("AppSettings:Audience").Value,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(10),
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokendescriptor);

        }
    }
}
