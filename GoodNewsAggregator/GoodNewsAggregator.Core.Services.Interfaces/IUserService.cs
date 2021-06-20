using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
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
    }
}
