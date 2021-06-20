using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> Register(UserDto userDto, string password)
        {
            var user = _mapper.Map<User>(userDto);

            var resultCreating = await _userManager.CreateAsync(user, password);
            if (resultCreating.Succeeded)
            {
                var resultAdding = await _userManager.AddToRoleAsync(user, userDto.Role);
                return resultAdding;
            }

            return resultCreating;
        }

        public async Task<UserDto> Login(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName);
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = await GetRoles(user);

                return userDto;
            }

            return null;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = await GetRoles(user);

            return userDto;
        }

        public async Task<UserDto> GetByUserName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = await GetRoles(user);

            return userDto;
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claims)
        {
            return _mapper.Map<UserDto>(await _userManager.GetUserAsync(claims));
        }

        public async Task<string> GetRolesOfUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            return await GetRoles(user);
        }

        private async Task<string> GetRoles(User user)
        {
            return (await _userManager.GetRolesAsync(user)).Aggregate((a, b) => a + ", " + b);
        }
    }
}
