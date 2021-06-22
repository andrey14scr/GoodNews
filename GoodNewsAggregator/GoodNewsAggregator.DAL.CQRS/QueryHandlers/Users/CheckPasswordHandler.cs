using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class CheckPasswordHandler : IRequestHandler<CheckPasswordQuery, bool>
    {
        private readonly UserManager<User> _userManager;

        public CheckPasswordHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(CheckPasswordQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new UserNotFoundException($"User with email {request.Email} not found");

            return await _userManager.CheckPasswordAsync(user, request.Password);
        }
    }
}