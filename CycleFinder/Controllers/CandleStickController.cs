using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Models;
using CycleFinder.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CycleFinder.Dtos;
using LazyCache;
using CycleFinder.Extensions;
using CycleFinder.Calculations;
using CycleFinder.Services;
using System.Drawing;
using CycleFinder.Models.Candles;
using CycleFinder.Calculations.Services;
using CycleFinder.Models.Extensions;

namespace CycleFinder.Controllers
{
    //TODO this class needs to be refactored into multiple controllers as it grows in complexity
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickController : ControllerBase
    {
        protected readonly ILogger<CandleStickController> Logger;
        protected readonly ICandleStickRepository CandleStickRepository;
        protected readonly IAppCache Cache;

        public CandleStickController(
            ILogger<CandleStickController> logger, 
            ICandleStickRepository candleStickRepository, 
            IAppCache cache)
        {
            Logger = logger;
            CandleStickRepository = candleStickRepository;
            Cache = cache;
        }

        protected Task<IEnumerable<CandleStick>> GetOrAddAllData(string symbol)
        {
            return Cache.GetOrAddAsync($"candles_{symbol}_{TimeFrame.Daily.GetDescription()}", () => CandleStickRepository.GetAllData(symbol, TimeFrame.Daily));
        }


        protected bool CheckSymbolExists(string symbol) => GetSymbols().Result.FirstOrDefault(_ => _.Name == symbol) != null;


        /// <summary>
        /// Gets all symbols available on the exchange.
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Symbol>> GetSymbols()
        {
            return await Cache.GetOrAddAsync("symbols", () => CandleStickRepository.ListSymbols(), TimeSpan.FromDays(1));
        }

        /// <summary>
        /// Gets all available data for a given instrument in Daily resolution.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the instrument.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickDto>> > GetAllData([FromQuery]string symbol)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok((await GetOrAddAllData(symbol)).Select(_ => _.ToCandleStickDto()));
        }

    }
}
