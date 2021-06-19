using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class GetUserMyEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetUserMyEmailHandler(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserDto>(await _userManager.FindByEmailAsync(request.Email));
        }
    }
}