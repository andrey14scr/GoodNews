using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodNewsAggregator.WebAPI.Auth
{
    public interface IJwtAuthManager
    {
        public JwtAuthResult GenerateToken(string email, Claim[] claims);
    }
}