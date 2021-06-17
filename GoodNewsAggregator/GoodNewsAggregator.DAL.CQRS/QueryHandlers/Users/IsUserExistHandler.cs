using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class IsUserExistHandler : IRequestHandler<IsUserExistQuery, bool>
    {
        private readonly UserManager<User> _userManager;

        public IsUserExistHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(IsUserExistQuery request, CancellationToken cancellationToken)
        {
            return await _userManager.FindByEmailAsync(request.Email) != null;
        }
    }
}