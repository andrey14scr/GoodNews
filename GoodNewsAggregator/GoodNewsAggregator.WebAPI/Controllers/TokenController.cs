using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Tools;
using GoodNewsAggregator.WebAPI.Auth;
using Microsoft.AspNetCore.Authorization;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public TokenController(IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            JwtAuthResult jwtResult;
            Claim[] claims;

            var userModel = await _userService.Login(request.Username, request.Password);

            if (userModel != null)
            {
                if (userModel.Role.ContainsRole(Constants.RoleNames.ADMIN))
                {
                    claims = new[]
                    {
                        new Claim(ClaimTypes.Role, userModel.Email),
                        new Claim(ClaimTypes.Role, Constants.RoleNames.ADMIN)
                    };
                }
                else
                {
                    claims = new[]
                    {
                        new Claim(ClaimTypes.Role, userModel.Email),
                        new Claim(ClaimTypes.Role, Constants.RoleNames.USER)
                    };
                }

                jwtResult = await _jwtAuthManager.GenerateToken(userModel.Email, claims);

                return Ok(jwtResult);
            }

            return Ok("not logined");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
