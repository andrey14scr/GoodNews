using System;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task AddOrUpdate(Guid id, string userName, DateTime expireAt);

        Task<RefreshToken> GetRefreshTokenByUserName(string userName);

        Task Remove(RefreshToken refreshToken);
    }
}