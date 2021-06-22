using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
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

        public async Task<IdentityResult> Register(UserDto userDto, string password)
        {
            return await _mediator.Send(new RegisterUserCommand(userDto, password));
        }

        public async Task<UserDto> Login(string userName, string password)
        {
            return await _mediator.Send(new LoginUserQuery(userName, password));
        }

        public async Task<UserDto> GetByUserName(string userName)
        {
            return await _mediator.Send(new GetUserByUserNameQuery(userName));
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            return await _mediator.Send(new GetUserByEmailQuery(email));
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

        public async Task<bool> CheckPassword(string password, string email)
        {
            return await _mediator.Send(new CheckPasswordQuery(password, email));
        }

        public async Task<EnumUserResults> FindSuchUser(Guid? id, string email, string userName)
        {
            return await _mediator.Send(new FindSuchUserQuery(id, email, userName));
        }

        public async Task<UserDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetUserByIdQuery(id));
        }
    }
}