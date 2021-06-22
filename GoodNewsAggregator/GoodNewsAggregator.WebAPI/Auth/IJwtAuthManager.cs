using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public interface IJwtAuthManager
    {
        public Task<JwtAuthResult> GenerateToken(UserDto userDto, DateTime now);
        public Task<JwtAuthResult> Refresh(RefreshToken refreshToken, string accessToken, DateTime now);
        public Task RemoveRefreshTokenByUserId(Guid userId);
    }
}