using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.Internal;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.Tools;
using GoodNewsAggregator.WebAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly UserManager<User> _userManager;

        public TokenController(IUserService userService, IJwtAuthManager jwtAuthManager, IRefreshTokenService refreshTokenService, UserManager<User> userManager)
        {
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _refreshTokenService = refreshTokenService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                UserDto userDto = await _userService.GetByUserName(request.UserName);
                var isCorrect = await _userService.CheckPassword(request.UserName, request.Password);
                
                if (isCorrect)
                {
                    JwtAuthResult jwtResult = await _jwtAuthManager.GenerateToken(userDto, DateTime.Now);
                    return Ok(jwtResult);
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

        [AllowAnonymous]
        [HttpPost]
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
                    return Ok(jwtResult);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                UserDto userDto = await _userService.GetByUserName(request.UserName);
                JwtAuthResult jwtResult = await _jwtAuthManager.GenerateToken(userDto, DateTime.Now);
                return Ok(jwtResult);
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
