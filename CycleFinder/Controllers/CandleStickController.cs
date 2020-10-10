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

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickController : ControllerBase
    {
        private readonly ILogger<CandleStickController> _logger;
        private readonly ICandleStickRepository _repository;
        private readonly IAppCache _cache;
        private readonly Func<IRandomColorGenerator> _colorGeneratorFactory;

        public CandleStickController(
            ILogger<CandleStickController> logger, 
            ICandleStickRepository repository, 
            IAppCache cache,
            Func<IRandomColorGenerator> colorGeneratorFactory)
        {
            _logger = logger;
            _repository = repository;
            _cache = cache;
            _colorGeneratorFactory = colorGeneratorFactory;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickDto>> > GetAllData([FromQuery]string symbol)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok((await GetOrAddAllData(symbol)).Select(_ => _.ToCandleStickDto()));
        }

        /// <summary>
        /// Gets all significant low points of the candlestick series as defined by the order parameter.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the instrument.</param>
        /// <param name="order">The order parameter defines the number of adjacent candles, both left and right, from a low for it to be considered valid.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLows([FromQuery]string symbol, [FromQuery]int order = 15)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok(
                await Task.Run(
                    async () => CandleStickMath.GetLocalMinima(
                        await GetOrAddAllData(symbol), order).Select(_ => 
                        _.ToCandleStickMarkerDto(_colorGeneratorFactory().GetRandomColor(),"LOW", MarkerPosition.BelowBar, MarkerShape.ArrowUp))));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighs([FromQuery]string symbol, [FromQuery]int order = 15)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok(
                await Task.Run(
                    async () => CandleStickMath.GetLocalMaxima(
                        await GetOrAddAllData(symbol), order).Select(_ => 
                        _.ToCandleStickMarkerDto(_colorGeneratorFactory().GetRandomColor(), "HIGH", MarkerPosition.AboveBar, MarkerShape.ArrowDown))));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetPrimaryTimeCyclesFromLows([FromQuery]string symbol, [FromQuery]int order = 10, [FromQuery]int? numberOfLows = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            Func<IDictionary<CandleStick, IEnumerable<CandleStick>>, IEnumerable<KeyValuePair<CandleStick, IEnumerable<CandleStick>>>> lowSelector = 
                d => !numberOfLows.HasValue ? d.AsEnumerable() : d.TakeLast(numberOfLows.Value);

            return Ok(
                await Task.Run(
                    async () =>
                        {
                            var ret = new List<CandleStickMarkerDto>();
                            int lowId = 1;
                            foreach (var cycles in lowSelector(CandleStickMath.GetPrimaryTimeCyclesFromLows(await GetOrAddAllData(symbol), order)))
                            {
                                var color = _colorGeneratorFactory().GetRandomColor();
                                ret.Add(cycles.Key.ToCandleStickMarkerDto(color, $"LOW #{lowId}", MarkerPosition.BelowBar, MarkerShape.ArrowUp));

                                int turnId = 1;
                                foreach (var turn in cycles.Value)
                                {
                                    ret.Add(turn.ToCandleStickMarkerDto(color, $"TURN #{lowId}/{turnId}", MarkerPosition.AboveBar, MarkerShape.ArrowDown));
                                    turnId++;
                                }
                                lowId++;
                            }
                            return ret;

                        }));
        }

        private bool CheckSymbolExists(string symbol) => GetSymbols().Result.FirstOrDefault(_ => _.Name == symbol) != null;
        private Task<IEnumerable<CandleStick>> GetOrAddAllData(string symbol) => _cache.GetOrAddAsync(symbol, () => _repository.GetAllData(symbol, TimeFrame.Daily));



    }
}
