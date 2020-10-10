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

        private Func<IDictionary<CandleStick, IEnumerable<CandleStick>>,int?,IEnumerable<KeyValuePair<CandleStick, IEnumerable<CandleStick>>>> selector = 
            (d,i) => !i.HasValue ? d.AsEnumerable() : d.TakeLast(i.Value);

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
                        await GetOrAddAllData(symbol), order).Select(_ => CreateLowMarker(_, _colorGeneratorFactory().GetRandomColor()))));

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
                        await GetOrAddAllData(symbol), order).Select(_ => CreateHighMarker(_, _colorGeneratorFactory().GetRandomColor()))));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithTurns([FromQuery]string symbol, [FromQuery]int order = 15, [FromQuery]int? numberOfLows = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            Func<IEnumerable<CandleWithTurns>, IEnumerable<CandleWithTurns>> selector = d => !numberOfLows.HasValue ? d : d.TakeLast(numberOfLows.Value);

            return Ok(
                await Task.Run(
                    async () =>
                        {
                            var ret = new List<CandleStickMarkerDto>();
                            int lowId = 1;
                            foreach (var cwt in selector(CandleStickMath.GetPrimaryTimeCyclesFromLows(await GetOrAddAllData(symbol), order)))
                            {
                                var color = _colorGeneratorFactory().GetRandomColor();
                                ret.Add(CreateLowMarker(cwt.Candle, color, lowId));

                                int turnId = 1;
                                foreach (var turn in cwt.Turns)
                                {
                                    ret.Add(CreateHighTurnMarker(turn, color, lowId, turnId));
                                    turnId++;
                                }
                                lowId++;
                            }
                            return ret;

                        }));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? numberOfLows = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            Func<IDictionary<CandleStick, IEnumerable<CandleStick>>, IEnumerable<KeyValuePair<CandleStick, IEnumerable<CandleStick>>>> selector =
                d => !numberOfLows.HasValue ? d.AsEnumerable() : d.TakeLast(numberOfLows.Value);

            return Ok(
                await Task.Run(
                    async () =>
                    {
                        var ret = new List<CandleStickMarkerDto>();
                        int lowId = 1;
                        foreach (var cycles in selector(CandleStickMath.GetPrimaryTimeCyclesFromHighs(await GetOrAddAllData(symbol), order)))
                        {
                            var color = _colorGeneratorFactory().GetRandomColor();
                            ret.Add(CreateHighMarker(cycles.Key, color, lowId));

                            int turnId = 1;
                            foreach (var turn in cycles.Value)
                            {
                                ret.Add(CreateLowTurnMarker(turn, color, lowId, turnId));
                                turnId++;
                            }
                            lowId++;
                        }
                        return ret;

                    }));
        }

        private bool CheckSymbolExists(string symbol) => GetSymbols().Result.FirstOrDefault(_ => _.Name == symbol) != null;
        private Task<IEnumerable<CandleStick>> GetOrAddAllData(string symbol) => _cache.GetOrAddAsync(symbol, () => _repository.GetAllData(symbol, TimeFrame.Daily));
        private CandleStickMarkerDto CreateLowMarker(CandleStick candle, Color color, int? id = null) 
            => candle.ToCandleStickMarkerDto(color, $"LOW {(id == null ? "" : "#")}{id}", MarkerPosition.BelowBar, MarkerShape.ArrowUp);

        private CandleStickMarkerDto CreateHighMarker(CandleStick candle, Color color, int? id = null)
            => candle.ToCandleStickMarkerDto(color, $"HIGH {(id == null ? "" : "#")}{id}", MarkerPosition.AboveBar, MarkerShape.ArrowDown);

        private CandleStickMarkerDto CreateHighTurnMarker(CandleStick candle, Color color, int lowId, int turnId)
            => candle.ToCandleStickMarkerDto(color, $"TURN #{lowId}/{turnId}", MarkerPosition.AboveBar, MarkerShape.ArrowDown);

        private CandleStickMarkerDto CreateLowTurnMarker(CandleStick candle, Color color, int highId, int turnId)
            => candle.ToCandleStickMarkerDto(color, $"TURN #{highId}/{turnId}", MarkerPosition.BelowBar, MarkerShape.ArrowUp);


    }
}
