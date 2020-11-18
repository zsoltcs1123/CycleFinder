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
using CycleFinder.Models.Candles;
using CycleFinder.Models.Extensions;
using Microsoft.Extensions.Configuration;
using CycleFinder.Services;

namespace CycleFinder.Controllers
{
    //TODO this class needs to be refactored into multiple controllers as it grows in complexity
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickController : ControllerBase
    {
        protected readonly ILogger<CandleStickController> Logger;
        protected readonly ICandleStickRepository CandleStickRepository;
        protected readonly IQueryParameterProcessor ParameterProcessor;
        protected readonly IAppCache Cache;
        protected readonly bool CacheCandleStickData;

        public CandleStickController(
            ILogger<CandleStickController> logger, 
            ICandleStickRepository candleStickRepository,
            IQueryParameterProcessor queryParameterProcessor,
            IAppCache cache, 
            IConfiguration configuration)
        {
            Logger = logger;
            CandleStickRepository = candleStickRepository;
            ParameterProcessor = queryParameterProcessor;
            Cache = cache;

            bool.TryParse(configuration.GetSection("CacheSettings")["CacheCandleStickData"], out bool cacheCandleStickData);
            CacheCandleStickData = cacheCandleStickData;
        }

        protected Task<IEnumerable<CandleStick>> GetOrAddAllData(string symbol, TimeFrame timeFrame)
        {
            if (CacheCandleStickData)
            {
                return Cache.GetOrAddAsync($"candles_{symbol}_{timeFrame.GetDescription()}", () => CandleStickRepository.GetAllData(symbol, timeFrame));
            }

            return CandleStickRepository.GetAllData(symbol, timeFrame);
        }


        protected bool CheckSymbolExists(string symbol) => GetSymbols().Result.FirstOrDefault(_ => _.Name == symbol) != null;
        protected bool CheckTimeFrameExists(string timeFrameStr, out TimeFrame? timeFrame)
        {
            var tf = ParameterProcessor.TimeFrameFromString(timeFrameStr);

            timeFrame = tf ?? null;

            return tf.HasValue;
        }


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
        public async Task<ActionResult<IEnumerable<CandleStickDto>> > GetAllData([FromQuery]string symbol, [FromQuery]string timeFrame)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            if (!CheckTimeFrameExists(timeFrame, out TimeFrame? tf))
            {
                return NotFound();
            }

            return Ok((await GetOrAddAllData(symbol, tf.Value)).Select(_ => _.ToCandleStickDto()));
        }

    }
}
