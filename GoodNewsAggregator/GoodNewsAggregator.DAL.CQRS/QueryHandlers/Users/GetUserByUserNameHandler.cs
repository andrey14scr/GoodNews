using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class GetUserByUserNameHandler : IRequestHandler<GetUserByUserNameQuery, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUserByUserNameHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                throw new UserNotFoundException($"User {request.UserName} not found");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = (await _userManager.GetRolesAsync(user)).Aggregate((a, b) => a + ", " + b);

            return userDto;
        }
    }
}