using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GoodNewsAggregator.WebAPI.Auth
{
    /// <summary>
    /// Service for jwt tokens management
    /// </summary>
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserService _userService;

        /// <summary>
        /// JwtAuthManager constructor
        /// </summary>
        /// <param name="configuration">App configuration</param>
        /// <param name="refreshTokenService">Refresh token service</param>
        /// <param name="userService">User service</param>
        public JwtAuthManager(IConfiguration configuration, IRefreshTokenService refreshTokenService, IUserService userService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _userService = userService;
        }

        /// <summary>
        /// Access token generation
        /// </summary>
        /// <param name="userDto">User dto</param>
        /// <param name="now">Current DateTime</param>
        /// <returns>Access token with its refresh token</returns>
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

        /// <summary>
        /// Refreshing existing access token
        /// </summary>
        /// <param name="accessToken">User's access token</param>
        /// <param name="now">Current DateTime</param>
        /// <returns>Access token with its refresh token</returns>
        public async Task<JwtAuthResult> Refresh(string accessToken, DateTime now)
        {
            var jwtToken = DecodeJwtToken(accessToken).JwtToken;

            var userName = jwtToken.Subject;
            if (string.IsNullOrEmpty(userName))
                throw new UserNotFoundException("Token's user not found");

            var userDto = await _userService.GetByUserName(userName);
            if (userDto == null)
                throw new UserNotFoundException($"User with {userName} not found");

            var refreshToken = await _refreshTokenService.GetRefreshTokenByUserId(userDto.Id);
            if (refreshToken == null)
                throw new SecurityTokenException("Refresh token not found");

            if (jwtToken == null || userName == null ||
                !jwtToken.Header.Alg.Equals(_configuration["Jwt:SecurityAlg"]) || 
                refreshToken.ExpireAt < now)
                throw new SecurityTokenException("Invalid token");

            return await GenerateToken(userDto, now);
        }

        /// <summary>
        /// Removing refresh token by userId
        /// </summary>
        /// <param name="userId">User's ID</param>
        public async Task RemoveRefreshTokenByUserId(Guid userId)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenByUserId(userId);
            await _refreshTokenService.Remove(refreshToken);
        }

        private (ClaimsPrincipal ClaimsPrincipal, JwtSecurityToken JwtToken) DecodeJwtToken(string token)
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
