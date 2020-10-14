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

            var lowMarkers = _candleStickCalculator.GetLocalMinima(await GetOrAddAllData(symbol), order).TakeLast(limit)
                .Select(_ => _.ToLowMarkerDto(_colorGeneratorFactory().GetRandomColor()));

            var highMarkers = _candleStickCalculator.GetLocalMaxima(await GetOrAddAllData(symbol), order).TakeLast(limit)
                .Select(_ => _.ToHighMarkerDto(_colorGeneratorFactory().GetRandomColor()));

            return Ok(lowMarkers.Concat(highMarkers).OrderBy(_ =>_.Time));
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
            var planetEnum = PlanetFromString(planet);

            if (!CheckSymbolExists(symbol) || !CheckPlanetExists(planetEnum))
            {
                return NotFound();
            }

            return Ok(await GenerateExtremeMarkers(_candleStickCalculator.GetLocalMinima, MapperExtensions.ToLowMarkerWithPlanetDto, symbol, planetEnum, order, limit));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithPlanetPositions(
            [FromQuery] string symbol,
            [FromQuery] string planet,
            [FromQuery] int order = 15,
            [FromQuery] int? limit = null)
        {
            var planetEnum = PlanetFromString(planet);

            if (!CheckSymbolExists(symbol) || !CheckPlanetExists(planetEnum))
            {
                return NotFound();
            }

            return Ok(await GenerateExtremeMarkers(_candleStickCalculator.GetLocalMaxima, MapperExtensions.ToHighMarkerWithPlanetDto, symbol, planetEnum, order, limit));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremesWithPlanetPositions(
            [FromQuery] string symbol,
            [FromQuery] string planet,
            [FromQuery] int order = 15,
            [FromQuery] int? limit = null)
        {
            var lowMarkers = await GenerateExtremeMarkers(_candleStickCalculator.GetLocalMinima, MapperExtensions.ToLowMarkerWithPlanetDto, symbol, PlanetFromString(planet), order, limit);
            var highMarkers = await GenerateExtremeMarkers(_candleStickCalculator.GetLocalMaxima, MapperExtensions.ToHighMarkerWithPlanetDto, symbol, PlanetFromString(planet), order, limit);

            return Ok(lowMarkers.Concat(highMarkers).OrderBy(_ => _.Time));
        }

        private async Task<IEnumerable<CandleStickMarkerDto>> GenerateExtremeMarkers(
            Func<IEnumerable<CandleStick>, int, IEnumerable<CandleStick>> candleSelector,
            Func<CandleStick, Color, Planet, double, CandleStickMarkerDto> mapper,
            string symbol,
            Planet? planetEnum,
            int order,
            int? limit)
        {
            var ret = new List<CandleStickMarkerDto>();
            foreach (var candle in candleSelector(await GetOrAddAllData(symbol), order).TakeLast(limit))
            {
                ret.Add(mapper(candle,
                    _colorGeneratorFactory().GetRandomColor(),
                    planetEnum.Value,
                    (await _ephemerisEntryRepository.GetCoordinatesByTime(candle.Time, planetEnum.Value)).Longitude));
            }

            return ret;
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
