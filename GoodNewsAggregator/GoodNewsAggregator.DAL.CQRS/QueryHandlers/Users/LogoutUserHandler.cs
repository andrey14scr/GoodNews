using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class LogoutUserHandler : IRequestHandler<LogoutUserQuery, Unit>
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public LogoutUserHandler(IMapper mapper, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Unit.Value;
        }
    }
}