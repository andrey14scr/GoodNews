using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public interface IJwtAuthManager
    {
        public Task<JwtAuthResult> GenerateToken(string email, DateTime now);
        public Task<JwtAuthResult> Refresh(RefreshToken refreshToken, string accessToken, DateTime now);
        public Task RemoveRefreshTokenByUserName(string userName);
    }
}