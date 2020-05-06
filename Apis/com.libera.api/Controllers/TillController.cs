using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.libera.core;
using com.libera.models.Entities;
using com.libera.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace com.libera.api.Controllers
{
    [Route("till")]
    [ApiController]
    public class TillController : ControllerBase
    {
        private readonly ITillService tillService;
        private readonly ILogger logger;
        public TillController(ITillService _tillService)
        {
            tillService = _tillService;
        }
        [HttpGet]
        public async Task<IStandardReply<IEnumerable<Coin>>> Get()
        {
            return await tillService.GetTillAsync(UserId, ApplicationId);
        }
        [HttpGet("change/{amount:decimal}")]
        public async Task<IStandardReply<IEnumerable<Coin>>> GetChange(decimal amount)
        {
            return await tillService.GetCorrectChangeAsync(amount, UserId, ApplicationId);
        }
        [HttpGet("amount")]
        public async Task<IStandardReply<decimal>> GetAmount()
        {
            return await tillService.GetTillCurrentAmount(UserId, ApplicationId);
        }
        [HttpPut]
        public async Task<IStandardReply<IEnumerable<Coin>>> Put([FromBody] List<Coin> coins)
        {
            await tillService.ClearTillAsync(UserId, ApplicationId);
            return await tillService.FillTillAsync(coins, UserId, ApplicationId);
        }
        [HttpPatch]
        public async Task<IStandardReply<IEnumerable<Coin>>> Patch([FromBody] List<Coin> coins)
        {
            return await tillService.FillTillAsync(coins, UserId, ApplicationId);
        }
    }
}