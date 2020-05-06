using com.libera.core;
using com.libera.models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.libera.services
{
    public interface ICoinTypeService
    {
        Task<IStandardReply<IEnumerable<CoinType>>> GetCoinTypesAsync(long userId, long applicationId);
        Task<IStandardReply<CoinType>> GetCoinTypeAsync(long id, long userId, long applicationId);
        Task<IStandardReply<CoinType>> GetCoinTypeAsync(string name, long userId, long applicationId);
        Task<IStandardReply<CoinType>> GetCoinTypeAsync(decimal value, long userId, long applicationId);
        Task<IStandardReply<CoinType>> AddCoinTypeAsync(CoinType type, long userId, long applicationId);
        Task<IStandardReply<CoinType>> AddCoinTypeAsync(string typeName, decimal value, long userId, long applicationId);
        Task<IStandardReply<CoinType>> RemoveCoinTypeAsync(long id, long userId, long applicationId);
        Task<IStandardReply<CoinType>> RemoveCoinTypeAsync(string name, long userId, long applicationId);
        Task<IStandardReply<CoinType>> RemoveCoinTypeAsync(decimal value, long userId, long applicationId);
    }
}
