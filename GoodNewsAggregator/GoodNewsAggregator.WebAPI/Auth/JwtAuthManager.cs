using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly IConfiguration _configuration;

        public JwtAuthManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtAuthResult GenerateToken(string email, Claim[] claims)
        {
            var jwtToken = new JwtSecurityToken("GoodNesAggregator",
                "GoodNesAggregator",
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken()
            {
                Email = email,
                ExpireAt = DateTime.Now.AddMinutes(60),
                Token = Guid.NewGuid().ToString("D")
            };

            // to bd

            return new JwtAuthResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }

    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }

    public class RefreshToken
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
    }

    public interface IJwtAuthManager
    {
        public JwtAuthResult GenerateToken(string email, Claim[] claims);
    }
}
