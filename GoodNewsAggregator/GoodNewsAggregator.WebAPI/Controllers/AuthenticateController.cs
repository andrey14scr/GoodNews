using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.WebAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with users and their authentication
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(200, Type = typeof(string))]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        /// <summary>
        /// AuthenticateController constructor
        /// </summary>
        public AuthenticateController(IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="request">Request consists of username and password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var userDto = await _userService.GetByUserName(request.UserName);
                var isCorrect = await _userService.CheckPassword(request.Password, userDto.Email);
                
                if (isCorrect)
                {
                    JwtAuthResult jwtResult = await _jwtAuthManager.GenerateToken(userDto, DateTime.Now);
                    return Ok(jwtResult.AccessToken);
                }

                return BadRequest("Password is not correct");
            }
            catch (UserNotFoundException e)
            {
                Log.Error(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerModel">Registration model with email, username, password and password confirmation</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var findResult = await _userService.FindSuchUser(null, registerModel.Email, registerModel.UserName);
            switch (findResult)
            {
                case EnumUserResults.HasUserWithSuchEmail:
                    return BadRequest("User with such email already exists");
                case EnumUserResults.HasUserWithSuchUserName:
                    return BadRequest("User with such username already exists");
            }

            var userDto = new UserDto()
            {
                Id = Guid.NewGuid(),
                Email = registerModel.Email,
                Role = Constants.RoleNames.USER,
                UserName = registerModel.UserName
            };

            try
            {
                var result = await _userService.Register(userDto, registerModel.Password);
                if (result.Succeeded)
                {
                    JwtAuthResult jwtResult = await _jwtAuthManager.GenerateToken(userDto, DateTime.Now);
                    return Ok(jwtResult.AccessToken);
                }

                return BadRequest(result.Errors);
            }
            catch (UserExistException e)
            {
                Log.Error(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Refresh your access token
        /// </summary>
        /// <param name="request">Request consists of username and access token</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                UserDto userDto = await _userService.GetByUserName(request.UserName);
                JwtAuthResult jwtResult = await _jwtAuthManager.GenerateToken(userDto, DateTime.Now);
                return Ok(jwtResult.AccessToken);
            }
            catch (UserNotFoundException e)
            {
                Log.Error(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest();
            }
        }
    }
}
