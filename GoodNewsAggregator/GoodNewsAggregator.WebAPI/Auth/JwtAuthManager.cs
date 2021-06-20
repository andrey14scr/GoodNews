using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public JwtAuthManager(IConfiguration configuration, IRefreshTokenService refreshTokenService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<JwtAuthResult> GenerateToken(string userName, DateTime now)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var result = _signInManager.SignInAsync(user, false);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            }.Union(userClaims).Union(roleClaims);

            var jwtToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                ExpireAt = now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]))
            };

            await _refreshTokenService.AddOrUpdate(refreshToken.Id, userName, refreshToken.ExpireAt);

            return new JwtAuthResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public Task<JwtAuthResult> Refresh(RefreshToken refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);

            var userName = principal.Identity?.Name;

            if (jwtToken == null || 
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature) || 
                refreshToken.ExpireAt < now ||
                refreshToken.UserName != userName)
                throw new SecurityTokenException("Invalid token");

            return GenerateToken(userName, now);
        }

        public async Task RemoveRefreshTokenByUserName(string userName)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenByUserName(userName);

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
