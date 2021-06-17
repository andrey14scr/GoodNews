using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.CQRS.Commands.Users;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class UserCqrsService : IUserService
    {
        private readonly IMediator _mediator;

        public UserCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IdentityResult> Register(UserDto userDto, string password, string role)
        {
            return await _mediator.Send(new RegisterUserCommand(userDto, password, role));
        }

        public async Task<SignInResult> Login(string userName, string password)
        {
            return await _mediator.Send(new LoginUserQuery(userName, password));
        }

        public async Task<bool> Exist(string email)
        {
            return await _mediator.Send(new IsUserExistQuery(email));
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claims)
        {
            return await _mediator.Send(new GetCurrentUserQuery(claims));
        }

        public async Task<string> GetRolesOfUser(UserDto userDto)
        {
            return await _mediator.Send(new GetUserRolesQuery(userDto));
        }

        public async Task Logout()
        {
            await _mediator.Send(new LogoutUserQuery());
        }
    }
}