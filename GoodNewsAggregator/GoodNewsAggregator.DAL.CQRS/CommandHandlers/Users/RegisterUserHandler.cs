using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
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
            if (request.UserDto == null)
                throw new NullReferenceException("UserDto was null");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new NullReferenceException("Input string(password) was empty");

            if (await _userManager.FindByIdAsync(request.UserDto.Id.ToString()) != null) 
                throw new UserExistException("User with such id already exists");

            if (!string.IsNullOrWhiteSpace(request.UserDto.Email))
            {
                if (await _userManager.FindByEmailAsync(request.UserDto.Email) != null)
                    throw new UserExistException("User with such email already exists");
            }

            if (!string.IsNullOrWhiteSpace(request.UserDto.UserName))
            {
                if (await _userManager.FindByNameAsync(request.UserDto.UserName) != null)
                    throw new UserExistException("User with such username already exists");
            }

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