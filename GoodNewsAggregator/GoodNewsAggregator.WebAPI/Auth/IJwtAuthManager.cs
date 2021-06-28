using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;

namespace GoodNewsAggregator.WebAPI.Auth
{
    /// <summary>
    /// Interface for work with jwt tokens
    /// </summary>
    public interface IJwtAuthManager
    {
        /// <summary>
        /// Access token generation
        /// </summary>
        /// <param name="userDto">User dto</param>
        /// <param name="now">Current DateTime</param>
        /// <returns>Access token with its refresh token</returns>
        public Task<JwtAuthResult> GenerateToken(UserDto userDto, DateTime now);

        /// <summary>
        /// Refreshing existing access token
        /// </summary>
        /// <param name="accessToken">User's access token</param>
        /// <param name="now">Current DateTime</param>
        /// <returns>Access token with its refresh token</returns>
        public Task<JwtAuthResult> Refresh(string accessToken, DateTime now);

        /// <summary>
        /// Removing refresh token by userId
        /// </summary>
        /// <param name="userId">User's ID</param>
        public Task RemoveRefreshTokenByUserId(Guid userId);
    }
}