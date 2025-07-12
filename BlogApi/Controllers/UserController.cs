using BlogApi.BLL.Dtos;
using BlogApi.BLL.Interfaces;
using BlogApi.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BlogApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("admin-login")]
        public async Task<IActionResult> Login([FromBody] UserDto entity)
        {
            var user = await _userService.GetIfIsAdmin(entity);

            if (user is null)
                return Unauthorized("Wrong username or password");

            var token = CreateToken(user);

            var response = new 
            {
                Token = token,
                ExpireAt = DateTime.UtcNow.AddMinutes(60),
                Username = user.UserName
            };

            return Ok(response);
        }
     
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
                );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokendescriptor = new JwtSecurityToken(
               issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
               audience: _configuration.GetValue<string>("AppSettings:Audience"),
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(60),
               signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(tokendescriptor);

        }
    }
}
