using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserService _userService;

        public JwtAuthManager(IConfiguration configuration, IRefreshTokenService refreshTokenService, IUserService userService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        public async Task<JwtAuthResult> GenerateToken(UserDto userDto, DateTime now)
        {
            if (userDto == null)
                throw new NullReferenceException("UserDto was null");

            var claims = new Claim[]
            {
                new (ClaimTypes.Name, userDto.UserName),
                new (ClaimTypes.Email, userDto.Email),
                new (ClaimTypes.Role, userDto.Role),
                new (JwtRegisteredClaimNames.Sub, userDto.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    _configuration["Jwt:SecurityAlg"]));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshTokenDto()
            {
                Id = Guid.NewGuid(),
                UserId = userDto.Id,
                ExpireAt = now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]))
            };

            await _refreshTokenService.AddOrUpdate(refreshToken);

            return new JwtAuthResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<JwtAuthResult> Refresh(RefreshToken refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);

            var userDto = await _userService.GetById(refreshToken.UserId);
            if (userDto == null)
                throw new UserNotFoundException($"User with id = {refreshToken.UserId} not found");

            var userName = principal.Identity?.Name;

            if (jwtToken == null || userName == null ||
                !jwtToken.Header.Alg.Equals(_configuration["Jwt:SecurityAlg"]) || 
                refreshToken.ExpireAt < now ||
                userDto.UserName != userName)
                throw new SecurityTokenException("Invalid token");

            return await GenerateToken(userDto, now);
        }

        public async Task RemoveRefreshTokenByUserId(Guid userId)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenByUserId(userId);
            await _refreshTokenService.Remove(refreshToken);
        }

        private (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new SecurityTokenException("Invalid token");

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken);
        }
    }
}
