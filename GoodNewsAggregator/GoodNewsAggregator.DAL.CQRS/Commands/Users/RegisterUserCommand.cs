using GoodNewsAggregator.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Users
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public UserDto UserDto { get; set; }
        public string Password { get; set; }

        public RegisterUserCommand(UserDto userDto, string password)
        {
            UserDto = userDto;
            Password = password;
        }
    }
}