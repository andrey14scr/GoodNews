using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> Register(UserDto userDto, string password, string role);
        public Task<SignInResult> Login(string userName, string password);
        public Task<Boolean> Exist(string email);
        public Task<UserDto> GetCurrentUser(ClaimsPrincipal claims);
        public Task<string> GetRolesOfUser(UserDto userDto);
        public Task Logout();
    }
}
