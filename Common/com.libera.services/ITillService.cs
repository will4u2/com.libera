using com.libera.core;
using com.libera.models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.libera.services
{
    public interface ITillService
    {
        /// <summary>
        /// Replaces till with coins provided
        /// </summary>
        /// <param name="coins"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> SwapTillAsync(IEnumerable<Coin> coins, long userId, long applicationId);
        /// <summary>
        /// Adds coins to till
        /// </summary>
        /// <param name="coins"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> FillTillAsync(IEnumerable<Coin> coins, long userId, long applicationId);
        /// <summary>
        /// Adds single coin to till
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        Task<IStandardReply<Coin>> AddCoinToTillAsync(Coin coin, long userId, long applicationId);
        /// <summary>
        /// Adds coins by quantity provided
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(Coin coin, int quantity, long userId, long applicationId);
        /// <summary>
        /// Adds coins by quantity and type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(CoinType type, int quantity, long userId, long applicationId);
        /// <summary>
        /// Adds coins by quantity and typeid
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(long typeId, int quantity, long userId, long applicationId);
        /// <summary>
        /// Adds coins by quantity and type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> AddCoinToTillAsync(string type, int quantity, long userId, long applicationId);
        /// <summary>
        /// Get current till
        /// </summary>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> GetTillAsync(long userId, long applicationId);
        /// <summary>
        /// Get optimized coin collection for change
        /// </summary>
        /// <param name="amountToChange"></param>
        /// <returns></returns>
        Task<IStandardReply<IEnumerable<Coin>>> GetCorrectChangeAsync(decimal amountToChange, long userId, long applicationId);
        /// <summary>
        /// Only added for pretest clean up
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task ClearTillAsync(long userId, long applicationId);
    }
}
