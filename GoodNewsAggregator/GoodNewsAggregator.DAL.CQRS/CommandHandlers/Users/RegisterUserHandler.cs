using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Users
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public RegisterUserHandler(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.UserDto);

            var resultCreating = await _userManager.CreateAsync(user, request.Password);
            if (resultCreating.Succeeded)
            {
                var resultAdding = await _userManager.AddToRoleAsync(user, request.UserDto.Role);
                return resultAdding;
            }

            return resultCreating;
        }
    }
}