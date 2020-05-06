using com.libera.core;
using com.libera.models.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.libera.services
{
    public class CoinTypeService : BaseService, ICoinTypeService
    {
        private readonly ILogger logger;
        public CoinTypeService(IRepository _repo, ILogger<CoinTypeService> _logger) : base(_repo)
        {
            logger = _logger;
        }
        public async Task<IStandardReply<CoinType>> AddCoinTypeAsync(CoinType type, long userId, long applicationId)
        {
            return await repo.SaveAsync<CoinType>(type, 1, 1, c => c.Value == type.Value);
        }

        public async Task<IStandardReply<CoinType>> AddCoinTypeAsync(string typeName, decimal value, long userId, long applicationId)
        {
            CoinType coinType = new CoinType { Type = typeName, Value = value };
            return await AddCoinTypeAsync(coinType, userId, applicationId);
        }

        public async Task<IStandardReply<CoinType>> GetCoinTypeAsync(long id, long userId, long applicationId)
        {
            return await repo.GetSingleAsync<CoinType>(id, userId, applicationId);
        }

        public async Task<IStandardReply<CoinType>> GetCoinTypeAsync(string name, long userId, long applicationId)
        {
            return await repo.GetSingleAsync<CoinType>(ct => ct.Type.ToLower().Equals(name.ToLower()), userId, applicationId);
        }

        public async Task<IStandardReply<CoinType>> GetCoinTypeAsync(decimal value, long userId, long applicationId)
        {
            return await repo.GetSingleAsync<CoinType>(ct => ct.Value == value, userId, applicationId);
        }

        public async Task<IStandardReply<IEnumerable<CoinType>>> GetCoinTypesAsync(long userId, long applicationId)
        {
            return await repo.GetEnumerableAsync<CoinType>(null, userId, applicationId);
        }

        public async Task<IStandardReply<CoinType>> RemoveCoinTypeAsync(long id, long userId, long applicationId)
        {
            return await repo.DeleteAsync<CoinType>(id, userId, applicationId);
        }

        public async Task<IStandardReply<CoinType>> RemoveCoinTypeAsync(string name, long userId, long applicationId)
        {
            return await repo.DeleteAsync<CoinType>(ct => ct.Type.ToLower().Equals(name.ToLower()), userId, applicationId);
        }

        public async Task<IStandardReply<CoinType>> RemoveCoinTypeAsync(decimal value, long userId, long applicationId)
        {
            return await repo.DeleteAsync<CoinType>(ct => ct.Value == value, userId, applicationId);
        }
    }
}
