using BlogApi.BLL.Dtos;
using BlogApi.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<User?> GetIfIsAdmin(UserDto entity);
        Task<TokenResponseDto?> RefreshToken(RefreshTokenRequestDto request);
        Task<TokenResponseDto?> GenerateTokens(User user);
    }
}
