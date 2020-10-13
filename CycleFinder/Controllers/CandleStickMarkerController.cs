using CycleFinder.Calculations;
using CycleFinder.Calculations.Services;
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
        private readonly ICandleStickCalculator _candleStickCalculator;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public CandleStickMarkerController(
            ILogger<CandleStickController> logger,
            ICandleStickRepository repository,
            IAppCache cache,
            Func<IRandomColorGenerator> colorGeneratorFactory,
            ICandleStickCalculator candleStickCalculator,
            IEphemerisEntryRepository ephemerisEntryRepository) : base(logger, repository, cache)
        {
            _colorGeneratorFactory = colorGeneratorFactory;
            _candleStickCalculator = candleStickCalculator;
            _ephemerisEntryRepository = ephemerisEntryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLows([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var lowCandles = _candleStickCalculator.GetLocalMinima(await GetOrAddAllData(symbol), order).TakeLast(limit);

            return Ok(lowCandles.Select(_ =>_.ToLowMarkerDto(_colorGeneratorFactory().GetRandomColor())));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighs([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var highCandles = _candleStickCalculator.GetLocalMinima(await GetOrAddAllData(symbol), order).TakeLast(limit);

            return Ok(highCandles.Select(_ => _.ToHighMarkerDto(_colorGeneratorFactory().GetRandomColor())));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremes([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var lowCandles = _candleStickCalculator.GetLocalMinima(await GetOrAddAllData(symbol), order).TakeLast(limit);
            var highCandles = _candleStickCalculator.GetLocalMaxima(await GetOrAddAllData(symbol), order).TakeLast(limit);

            return Ok(lowCandles.Concat(highCandles).OrderBy(_ =>_.Time).Select(_ => _.ToHighMarkerDto(_colorGeneratorFactory().GetRandomColor())));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var lowsWithTurns = _candleStickCalculator.GetPrimaryTimeCyclesFromLows(await GetOrAddAllData(symbol), order).TakeLast(limit);

            var ret = new List<CandleStickMarkerDto>();
            int lowId = 1;
            foreach (var cwt in lowsWithTurns)
            {
                var color = _colorGeneratorFactory().GetRandomColor();
                ret.Add(cwt.Candle.ToLowMarkerDto(color, lowId));

                int turnId = 1;
                foreach (var turn in cwt.Turns)
                {
                    ret.Add(turn.ToHighTurnMarkerDto(color, lowId, turnId));
                    turnId++;
                }
                lowId++;
            }

            return Ok(ret);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var highsWithTurns = _candleStickCalculator.GetPrimaryTimeCyclesFromHighs(await GetOrAddAllData(symbol), order).TakeLast(limit);

            var ret = new List<CandleStickMarkerDto>();
            int lowId = 1;
            foreach (var cwt in highsWithTurns)
            {
                var color = _colorGeneratorFactory().GetRandomColor();
                ret.Add(cwt.Candle.ToHighMarkerDto(color, lowId));

                int turnId = 1;
                foreach (var turn in cwt.Turns)
                {
                    ret.Add(turn.ToLowTurnMarkerDto(color, lowId, turnId));
                    turnId++;
                }
                lowId++;
            }

            return Ok(ret);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithPlanetPositions(
            [FromQuery] string symbol, 
            [FromQuery] string planet, 
            [FromQuery] int order = 15, 
            [FromQuery] int? limit = null)
        {
            return await GetExtremeWithPlanetPositions(_candleStickCalculator.GetLocalMinima, symbol, planet, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithPlanetPositions(
            [FromQuery] string symbol,
            [FromQuery] string planet,
            [FromQuery] int order = 15,
            [FromQuery] int? limit = null)
        {
            return await GetExtremeWithPlanetPositions(_candleStickCalculator.GetLocalMaxima, symbol, planet, order, limit);
        }

        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremeWithPlanetPositions(
            Func<IEnumerable<CandleStick>, int, IEnumerable<CandleStick>> candleSelector,
            string symbol, 
            string planet, 
            int order,
            int? limit)
        {
            var planetEnum = PlanetFromString(planet);

            if (!CheckSymbolExists(symbol) || !CheckPlanetExists(planetEnum))
            {
                return NotFound();
            }

            var ret = new List<CandleStickMarkerDto>();
            foreach (var candle in candleSelector(await GetOrAddAllData(symbol), order).TakeLast(limit))
            {
                ret.Add(candle.ToPlanetPositionMarkerDto(
                    _colorGeneratorFactory().GetRandomColor(),
                    planetEnum.Value,
                    (await _ephemerisEntryRepository.GetCoordinatesByTime(candle.Time, planetEnum.Value)).Longitude));
            }

            return Ok(ret);
        }


        private Planet? PlanetFromString(string planet) => planet switch
        {
            "moon" => Planet.Moon,
            "sun" => Planet.Sun,
            "mercury" => Planet.Mercury,
            "venus" => Planet.Venus,
            "mars" => Planet.Mars,
            "jupiter" => Planet.Jupiter,
            "saturn" => Planet.Saturn,
            "uranus" => Planet.Uranus,
            "neptune" => Planet.Neptune,
            "pluto" => Planet.Pluto,
            _ => null,
        };

        private bool CheckPlanetExists(Planet? planet) => planet.HasValue;
    }
}
