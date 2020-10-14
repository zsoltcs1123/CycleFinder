using CycleFinder.Calculations.Services;
using CycleFinder.Data;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Models;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Models.Candles;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickMarkerController : CandleStickController
    {
        private readonly Func<IRandomColorGenerator> _colorGeneratorFactory;
        private readonly ILocalExtremeCalculator _localExtremeCalculator;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly ICandleStickMarkerCalculator _candleStickMarkerCalculator;

        public CandleStickMarkerController(
            ILogger<CandleStickController> logger,
            ICandleStickRepository repository,
            IAppCache cache,
            Func<IRandomColorGenerator> colorGeneratorFactory,
            ILocalExtremeCalculator localExtremeCalculator,
            IEphemerisEntryRepository ephemerisEntryRepository,
            ICandleStickMarkerCalculator candleStickMarkerCalculator) : base(logger, repository, cache)
        {
            _colorGeneratorFactory = colorGeneratorFactory;
            _localExtremeCalculator = localExtremeCalculator;
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _candleStickMarkerCalculator = candleStickMarkerCalculator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLows([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var spec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.Low, symbol, order, limit), Extremes.Low, _colorGeneratorFactory());
            return Ok(ExecuteSpec(spec));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighs([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var spec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.High, symbol, order, limit), Extremes.High, _colorGeneratorFactory());
            return Ok(ExecuteSpec(spec));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremes([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var lowSpec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.Low, symbol, order, limit), Extremes.Low, _colorGeneratorFactory());
            var highSpec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.High, symbol, order, limit), Extremes.High, _colorGeneratorFactory());

            return Ok(ExecuteSpec(lowSpec).Concat(ExecuteSpec(highSpec)).OrderBy(_ => _.Time));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var spec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.Low, symbol, order, limit), Extremes.Low, _colorGeneratorFactory())
            {
                IncluePrimaryStaticCycles = true
            };

            return Ok(ExecuteSpec(spec));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var spec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.High, symbol, order, limit), Extremes.High, _colorGeneratorFactory())
            {
                IncluePrimaryStaticCycles = true
            };

            return Ok(ExecuteSpec(spec));
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

            var spec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.Low, symbol, order, limit), Extremes.Low, _colorGeneratorFactory())
            {
                IncludeLongitudinalReturns = true,
                Planets = planetEnum.Value
            };

            return Ok(ExecuteSpec(spec));
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

            var spec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.High, symbol, order, limit), Extremes.High, _colorGeneratorFactory())
            {
                IncludeLongitudinalReturns = true,
                Planets = planetEnum.Value
            };

            return Ok(ExecuteSpec(spec));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremesWithPlanetPositions(
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

            var lowSpec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.Low, symbol, order, limit), Extremes.Low, _colorGeneratorFactory())
            {
                IncludeLongitudinalReturns = true,
                Planets = planetEnum.Value
            };

            var highSpec = new CandleStickMarkerSpecification(await GetExtremes(Extremes.High, symbol, order, limit), Extremes.High, _colorGeneratorFactory())
            {
                IncludeLongitudinalReturns = true,
                Planets = planetEnum.Value
            };

            return Ok(ExecuteSpec(lowSpec).Concat(ExecuteSpec(highSpec)).OrderBy(_ => _.Time).OrderBy(_ => _.Time));
        }

        private async Task<IEnumerable<CandleStick>> GetExtremes(Extremes extreme, string symbol, int order, int? limit)
        {
            return extreme switch
            {
                Extremes.Low => _localExtremeCalculator.GetLocalMinima(await GetOrAddAllData(symbol), order).TakeLast(limit),
                Extremes.High => _localExtremeCalculator.GetLocalMaxima(await GetOrAddAllData(symbol), order).TakeLast(limit),
                _ => null,
            };
        }

        private IEnumerable<CandleStickMarkerDto> ExecuteSpec(CandleStickMarkerSpecification spec) => _candleStickMarkerCalculator.GetMarkers(spec).Select(_ => _.ToCandleStickMarkerDto());

        private Planets? PlanetFromString(string planet) => planet switch
        {
            "moon" => Planets.Moon,
            "sun" => Planets.Sun,
            "mercury" => Planets.Mercury,
            "venus" => Planets.Venus,
            "mars" => Planets.Mars,
            "jupiter" => Planets.Jupiter,
            "saturn" => Planets.Saturn,
            "uranus" => Planets.Uranus,
            "neptune" => Planets.Neptune,
            "pluto" => Planets.Pluto,
            _ => null,
        };

        private bool CheckPlanetExists(Planets? planet) => planet.HasValue;
    }
}
