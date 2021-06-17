using GoodNewsAggregator.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Users
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public UserDto UserDto { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public RegisterUserCommand(UserDto userDto, string password, string role)
        {
            UserDto = userDto;
            Password = password;
            Role = role;
        }
    }
}