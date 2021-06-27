using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class FindSuchUserHandler : IRequestHandler<FindSuchUserQuery, EnumUserResults>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public FindSuchUserHandler(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<EnumUserResults> Handle(FindSuchUserQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                if (await _userManager.FindByIdAsync(request.Id.ToString()) != null)
                    return EnumUserResults.HasUserWithSuchId;
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (await _userManager.FindByEmailAsync(request.Email) != null)
                    return EnumUserResults.HasUserWithSuchEmail;
            }

            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                if (await _userManager.FindByNameAsync(request.UserName) != null)
                    return EnumUserResults.HasUserWithSuchUserName;
            }

            return EnumUserResults.Good;
        }
    }
}