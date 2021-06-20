using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}