using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
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

        public async Task<IdentityResult> Register(UserDto userDto, string password, string role)
        {
            var isExist = await Exist(userDto.Email);
            if (isExist)
                return null;

            var user = _mapper.Map<User>(userDto);

            var resultCreating = await _userManager.CreateAsync(user, password);
            if (resultCreating.Succeeded)
            {
                var resultAdding = await _userManager.AddToRoleAsync(user, role);
                return resultAdding;
            }

            return resultCreating;
        }

        public async Task<SignInResult> Login(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            return result;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claims)
        {
            return _mapper.Map<UserDto>(await _userManager.GetUserAsync(claims));
        }

        public async Task<string> GetRolesOfUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            return (await _userManager.GetRolesAsync(user)).Aggregate((a, b) => a + ", " + b);
        }

        public async Task<Boolean> Exist(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
