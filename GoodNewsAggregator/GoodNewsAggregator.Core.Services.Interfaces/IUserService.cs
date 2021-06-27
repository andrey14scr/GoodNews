using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IdentityResult> Register(UserDto userDto, string password);
        public Task<UserDto> Login(string userName, string password);
        public Task<UserDto> GetByUserName(string userName);
        public Task<UserDto> GetByEmail(string email);
        public Task<UserDto> GetCurrentUser(ClaimsPrincipal claims);
        public Task<string> GetRolesOfUser(UserDto userDto);
        public Task Logout();
        public Task<bool> CheckPassword(string password, string email);
        public Task<EnumUserResults> FindSuchUser(Guid? id, string email, string userName);
        public Task<UserDto> GetById(Guid id);
    }
}
