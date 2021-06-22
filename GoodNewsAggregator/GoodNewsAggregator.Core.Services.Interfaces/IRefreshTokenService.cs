using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task AddOrUpdate(RefreshTokenDto refreshToken);

        Task<RefreshTokenDto> GetRefreshTokenByUserId(Guid userId);

        Task Remove(RefreshTokenDto refreshToken);
    }
}