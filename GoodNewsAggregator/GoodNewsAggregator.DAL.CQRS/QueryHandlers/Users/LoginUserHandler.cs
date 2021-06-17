using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Users
{
    public class LoginUserHandler : IRequestHandler<LoginUserQuery, SignInResult>
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public LoginUserHandler(IMapper mapper, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            return result;
        }
    }
}