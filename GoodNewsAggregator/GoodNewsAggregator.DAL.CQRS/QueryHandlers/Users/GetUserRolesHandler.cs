using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, string>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetUserRolesHandler(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.UserDto);
            return (await _userManager.GetRolesAsync(user)).Aggregate((a, b) => a + ", " + b);
        }
    }
}