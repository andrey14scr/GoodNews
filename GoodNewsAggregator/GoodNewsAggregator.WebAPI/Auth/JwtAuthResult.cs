using GoodNewsAggregator.Core.DTO;

namespace GoodNewsAggregator.WebAPI.Auth
{
    /// <summary>
    /// Result of token generation
    /// </summary>
    public class JwtAuthResult
    {
        /// <summary>
        /// Access token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Refresh token
        /// </summary>
        public RefreshTokenDto RefreshToken { get; set; }
    }
}