using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
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

        public async Task<IdentityResult> Register(UserDto userDto, string password)
        {
            if(userDto == null)
                throw new NullReferenceException("UserDto was null");

            if (string.IsNullOrWhiteSpace(password))
                throw new NullReferenceException("Input string(password) was empty");

            var findResult = await FindSuchUser(userDto.Id, userDto.Email, userDto.UserName);
            switch (findResult)
            {
                case EnumUserResults.HasUserWithSuchEmail:
                    throw new UserExistException("User with such email already exists");
                case EnumUserResults.HasUserWithSuchId:
                    throw new UserExistException("User with such id already exists");
                case EnumUserResults.HasUserWithSuchUserName:
                    throw new UserExistException("User with such username already exists");
            }

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

        public async Task<bool> CheckPassword(string password, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new UserNotFoundException($"User with email {email} not found");

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<EnumUserResults> FindSuchUser(Guid? id, string email, string userName)
        {
            if (id.HasValue)
            {
                if (await _userManager.FindByIdAsync(id.ToString()) != null)
                    return EnumUserResults.HasUserWithSuchId;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (await _userManager.FindByEmailAsync(email) != null)
                    return EnumUserResults.HasUserWithSuchEmail;
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                if (await _userManager.FindByNameAsync(userName) != null)
                    return EnumUserResults.HasUserWithSuchUserName;
            }

            return EnumUserResults.Good;
        }

        public async Task<UserDto> GetById(Guid id)
        {
            return _mapper.Map<UserDto>(await _userManager.FindByIdAsync(id.ToString()));
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new UserNotFoundException($"User with email {email} not found");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = await GetRoles(user);

            return userDto;
        }

        public async Task<UserDto> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                throw new UserNotFoundException($"User {userName} not found");

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
