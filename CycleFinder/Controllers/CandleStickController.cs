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

namespace CycleFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandleStickController : ControllerBase
    {
        private readonly ILogger<CandleStickController> _logger;
        private readonly ICandleStickRepository _repository;
        private readonly IAppCache _cache;

        public CandleStickController(
            ILogger<CandleStickController> logger, 
            ICandleStickRepository repository, 
            IAppCache cache)
        {
            _logger = logger;
            _repository = repository;
            _cache = cache;
        }

        /// <summary>
        /// Gets all symbols available on the exchange.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        private async Task<IEnumerable<Symbol>> GetSymbols()
        {
            return await _cache.GetOrAddAsync("symbols", () => _repository.ListSymbols(), TimeSpan.FromDays(1));
        }

        /// <summary>
        /// Gets all available data for a given instrument in Daily resolution.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the instrument.</param>
        /// <returns></returns>
        [HttpGet("{symbol}")]
        public async Task<ActionResult<IEnumerable<CandleStickDto>> > GetAllData(string symbol)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok((await GetOrAddAllData(symbol)).Select(_ => _.ToDto()));
        }

        /// <summary>
        /// Gets all significant low points of the candlestick series as defined by the order parameter.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the instrument.</param>
        /// <param name="order">The order parameter defines the number of adjacent candles, both left and right, from a low for it to be considered valid.</param>
        /// <returns></returns>
        [HttpGet("{symbol}, {order}")]
        public async Task<ActionResult<IEnumerable<CandleStickDto>>> GetLows(string symbol, int order = 5)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok(await Task.Run(async () => CandleStickMath.GetLocalMinima(await GetOrAddAllData(symbol),order)));
        }

        private bool CheckSymbolExists(string symbol) => GetSymbols().Result.FirstOrDefault(_ => _.Name == symbol) != null;
        private Task<IEnumerable<CandleStick>> GetOrAddAllData(string symbol) => _cache.GetOrAddAsync(symbol, () => _repository.GetAllData(symbol, TimeFrame.Daily));



    }
}
