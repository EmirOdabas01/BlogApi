using BlogApi.BLL.Dtos;
using BlogApi.BLL.Interfaces;
using BlogApi.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BlogApi.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthService userService, IConfiguration configuration)
        {
            _authService = userService;
            _configuration = configuration;
        }

        [HttpPost("admin-login")]
        public async Task<ActionResult<TokenResponseDto?>> Login([FromBody] UserDto entity)
        {
            var user = await _authService.GetIfIsAdmin(entity);

            if (user is null)
                return BadRequest("Wrong username or password");
            
            var result = await _authService.GenerateTokens(user);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string?>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshToken(request);

            if (result is null || result.RefreshToken is null || result.AccessToken is null)
                return Unauthorized("Invalid refresh token");

            return Ok(result.AccessToken);
        }
       
    }
}
