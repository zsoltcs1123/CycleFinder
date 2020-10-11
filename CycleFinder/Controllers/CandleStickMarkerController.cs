using CycleFinder.Calculations;
using CycleFinder.Data;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Models;
using CycleFinder.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickMarkerController : CandleStickController
    {
        private readonly Func<IRandomColorGenerator> _colorGeneratorFactory;
        private readonly Func<IEnumerable<CandleWithTurns>, int?, IEnumerable<CandleWithTurns>> _candleLimiter = 
            (candles, limit) => !limit.HasValue ? candles : candles.TakeLast(limit.Value);

        public CandleStickMarkerController(
            ILogger<CandleStickController> logger,
            ICandleStickRepository repository,
            IAppCache cache,
            Func<IRandomColorGenerator> colorGeneratorFactory) : base(logger, repository, cache)
        {
            _colorGeneratorFactory = colorGeneratorFactory;
        }

        /// <summary>
        /// Gets all significant low points of the candlestick series as defined by the order parameter.
        /// </summary>
        /// <param name="symbol">Ticker symbol of the instrument.</param>
        /// <param name="order">The order parameter defines the number of adjacent candles, both left and right, from a low for it to be considered valid.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLows([FromQuery] string symbol, [FromQuery] int order = 15)
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
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighs([FromQuery] string symbol, [FromQuery] int order = 15)
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
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok(
                await Task.Run(
                    async () =>
                    {
                        var ret = new List<CandleStickMarkerDto>();
                        int lowId = 1;
                        foreach (var cwt in _candleLimiter(CandleStickMath.GetPrimaryTimeCyclesFromLows(await GetOrAddAllData(symbol), order), limit))
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
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            return Ok(
                await Task.Run(
                    async () =>
                    {
                        var ret = new List<CandleStickMarkerDto>();
                        int lowId = 1;
                        foreach (var cwt in _candleLimiter(CandleStickMath.GetPrimaryTimeCyclesFromHighs(await GetOrAddAllData(symbol), order), limit))
                        {
                            var color = _colorGeneratorFactory().GetRandomColor();
                            ret.Add(CreateHighMarker(cwt.Candle, color, lowId));

                            int turnId = 1;
                            foreach (var turn in cwt.Turns)
                            {
                                ret.Add(CreateLowTurnMarker(turn, color, lowId, turnId));
                                turnId++;
                            }
                            lowId++;
                        }
                        return ret;

                    }));
        }

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
