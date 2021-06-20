using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.Tools;
using GoodNewsAggregator.WebAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public TokenController(IUserService userService, IJwtAuthManager jwtAuthManager, IRefreshTokenService refreshTokenService, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _refreshTokenService = refreshTokenService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            JwtAuthResult jwtResult;
            Claim[] claims;

            var user = await _userManager.FindByNameAsync(request.Username);
            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (result)
            {
                jwtResult = await _jwtAuthManager.GenerateToken(request.Username, DateTime.Now);

                return Ok(jwtResult);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userDto = new UserDto()
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Role = Constants.RoleNames.USER,
                UserName = model.UserName
            };

            var result = await _userService.Register(userDto, model.Password);
            
            if(result.Succeeded)
                return Ok();
            else
                return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var userName = User.Identity?.Name; // null
            await _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            await _userService.Logout();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenByUserName(request.Username);

            if (refreshToken == null)
                return BadRequest();

            var jwtResult = await _jwtAuthManager.Refresh(refreshToken, request.Token, DateTime.Now);

            return Ok(jwtResult);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RefreshRequest
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
