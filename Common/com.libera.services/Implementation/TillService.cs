using com.libera.core;
using com.libera.models.Entities;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace com.libera.services
{
    public class TillService : BaseService, ITillService
    {
        private readonly ILogger logger;
        private readonly ICoinTypeService coinTypeService;
        public TillService(IRepository _repo, ILogger<TillService> _logger, ICoinTypeService _coinTypeService) : base (_repo)
        {
            logger = _logger;
            coinTypeService = _coinTypeService;
        }

        public async Task<IStandardReply<Coin>> AddCoinToTillAsync(Coin coin, long userId, long applicationId)
        {
            //var result = await repo.GetSingleAsync<Coin>(coin.ID, userId, applicationId);
            //if(result != null && result.Success && result.Response != null)
            //{
            //    coin.Quantity += result.Response.Quantity;
            //}
            return await repo.SaveAsync<Coin>(coin, userId, applicationId, c => c.CoinTypeId == coin.CoinTypeId);
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(Coin coin, int quantity, long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> reply = StandardReply<IEnumerable<Coin>>.CreateInstance();
            coin.Quantity = quantity;
            try
            {
                var result = await AddCoinToTillAsync(coin, userId, applicationId);
                if (result.Success)
                {
                    reply = await GetTillAsync(userId, applicationId);
                }
                else
                {
                    reply.Messages.AddRange(result.Messages);
                    reply.Exceptions.AddRange(result.Exceptions);
                    reply.Success = false;
                }
            }
            catch(Exception exc)
            {
                reply.ProcessException(exc, coin, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(CoinType type, int quantity, long userId, long applicationId)
        {
            return await AddCoinToTillAsync(type.ID, quantity, userId, applicationId);
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(long typeId, int quantity, long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> reply = StandardReply<IEnumerable<Coin>>.CreateInstance();
            Coin coin = Coin.CreateInstance<Coin>(userId, applicationId);
            coin.Quantity = quantity;
            coin.CoinTypeId = typeId;
            return await AddCoinToTillAsync(coin, quantity, userId, applicationId);
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> FillTillAsync(IEnumerable<Coin> coins, long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> reply = StandardReply<IEnumerable<Coin>>.CreateInstance();
            try
            {
                coins.ToList().ForEach(async (coin) => {
                    var result = await AddCoinToTillAsync(coin, userId, applicationId);
                    if (!result.Success)
                    {
                        reply.Messages.AddRange(result.Messages);
                        reply.Exceptions.AddRange(result.Exceptions);
                        reply.Success = false;
                    }

                });
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, coins, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> GetCorrectChangeAsync(decimal amountToChange, long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> reply = StandardReply<IEnumerable<Coin>>.CreateInstance();
            try
            {
                List<Coin> change = new List<Coin>();
                while (amountToChange > 0)
                {
                    Coin currentCoin = await coinChecker(amountToChange, userId, applicationId);
                    change.Add(currentCoin);
                    amountToChange -= currentCoin.Quantity * currentCoin.Type.Value;
                }
                await AdjustTillAsync(change, userId, applicationId);
                reply.Response = change;
            }
            catch(Exception exc)
            {
                reply.ProcessException(exc, new { amountToChange }, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        private async Task<Coin> coinChecker(decimal amountToChange, long userId, long applicationId)
        {
            Coin usableCoin = new Coin();
            try
            {
                List<Coin> availableCoins = (await GetTillAsync(userId, applicationId)).Response.Where(c => c.Quantity > 0).OrderByDescending(c => c.Type.Value).ToList();
                usableCoin = availableCoins.FirstOrDefault(c => (amountToChange / c.Type.Value) > 1);
                if (usableCoin != null && usableCoin.Type != null)
                {
                    usableCoin.Quantity = (int)(amountToChange / usableCoin.Type.Value);
                }
                else
                {
                    throw new Exception("Not able to make change.");
                }
            }
            catch(Exception exc)
            {
                throw new Exception("Not able to make change.");
            }
            return usableCoin;
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> GetTillAsync(long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> result = await repo.GetEnumerableAsync<Coin>(null, userId, applicationId);
            result.Response.ToList().ForEach(async (coin) => {
                coin.Type = (await coinTypeService.GetCoinTypeAsync(coin.CoinTypeId, userId, applicationId)).Response;
            });
            return result;
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> SwapTillAsync(IEnumerable<Coin> coins, long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> reply = StandardReply<IEnumerable<Coin>>.CreateInstance();
            try
            {
                (await GetTillAsync(userId, applicationId)).Response.ToList().ForEach(async (coin) => {
                    var result = await DeleteAsync<Coin>(coin.ID, userId, applicationId);
                    reply.Messages.AddRange(result.Messages);
                    reply.Exceptions.AddRange(result.Exceptions);
                });
                reply = await FillTillAsync(coins, userId, applicationId);
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, coins, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(string type, int quantity, long userId, long applicationId)
        {
            CoinType coinType = (await coinTypeService.GetCoinTypeAsync(type, userId, applicationId)).Response;
            return await AddCoinToTillAsync(coinType.ID, quantity, userId, applicationId);
        }

        public async Task ClearTillAsync(long userId, long applicationId)
        {
            await Connection.DeleteAllAsync<Coin>();
        }

        public async Task<IStandardReply<decimal>> GetTillCurrentAmount(long userId, long applicationId)
        {
            IStandardReply<decimal> reply = StandardReply<decimal>.CreateInstance();
            var till = await GetTillAsync(userId, applicationId);
            reply.Response = till.Response.Sum(t => t.Quantity * t.Type.Value);
            return reply;
        }

        public async Task<IStandardReply<IEnumerable<Coin>>> AdjustTillAsync(IEnumerable<Coin> coins, long userId, long applicationId)
        {
            IStandardReply<IEnumerable<Coin>> reply = StandardReply<IEnumerable<Coin>>.CreateInstance();
            try
            {
                coins.ToList().ForEach(async (coin) => {
                    Coin lc = (await repo.GetSingleAsync<Coin>(coin.ID, userId, applicationId)).Response;
                    lc.Quantity -= coin.Quantity;
                    await repo.SaveAsync<Coin>(lc, userId, applicationId);
                });
            }
            catch (Exception exc)
            {
                reply.ProcessException(exc, coins, logger, MethodBase.GetCurrentMethod().Name, false);
            }
            return reply;
        }
    }
}
