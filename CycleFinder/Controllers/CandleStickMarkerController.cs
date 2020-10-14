using CycleFinder.Calculations.Services;
using CycleFinder.Data;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Models;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly ICandleStickMarkerCalculator _candleStickMarkerCalculator;

        public CandleStickMarkerController(
            ILogger<CandleStickController> logger,
            ICandleStickRepository repository,
            IAppCache cache,
            IEphemerisEntryRepository ephemerisEntryRepository,
            ICandleStickMarkerCalculator candleStickMarkerCalculator) : base(logger, repository, cache)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _candleStickMarkerCalculator = candleStickMarkerCalculator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLows([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var spec = new CandleStickMarkerSpecification
            {
                Extremes = Extremes.Low
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighs([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {

            var spec = new CandleStickMarkerSpecification
            {
                Extremes = Extremes.High
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremes([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var specs = new[]
            {
                new CandleStickMarkerSpecification
                {
                    Extremes = Extremes.Low
                },
                new CandleStickMarkerSpecification
                {
                    Extremes = Extremes.High
                }
            };

            return await ProcessSpecs(specs, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var spec = new CandleStickMarkerSpecification
            {
                Extremes = Extremes.Low,
                IncluePrimaryStaticCycles = true
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var spec = new CandleStickMarkerSpecification
            {
                Extremes = Extremes.High,
                IncluePrimaryStaticCycles = true
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithPlanetPositions(
            [FromQuery] string symbol, 
            [FromQuery] string planet, 
            [FromQuery] int order = 15, 
            [FromQuery] int? limit = null)
        {
            var planetEnum = PlanetFromString(planet);

            if (!CheckPlanetExists(planetEnum))
            {
                return NotFound();
            }

            var spec = new CandleStickMarkerSpecification
            {
                Extremes = Extremes.Low,
                IncludeLongitudinalReturns = true,
                Planets = planetEnum.Value
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithPlanetPositions(
            [FromQuery] string symbol,
            [FromQuery] string planet,
            [FromQuery] int order = 15,
            [FromQuery] int? limit = null)
        {
            var planetEnum = PlanetFromString(planet);

            if (!CheckPlanetExists(planetEnum))
            {
                return NotFound();
            }

            var spec = new CandleStickMarkerSpecification
            {
                Extremes = Extremes.High,
                IncludeLongitudinalReturns = true,
                Planets = planetEnum.Value
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremesWithPlanetPositions(
            [FromQuery] string symbol,
            [FromQuery] string planet,
            [FromQuery] int order = 15,
            [FromQuery] int? limit = null)
        {
            var planetEnum = PlanetFromString(planet);

            if (!CheckPlanetExists(planetEnum))
            {
                return NotFound();
            }

            var specs = new[]
{
                new CandleStickMarkerSpecification
                {
                    Extremes = Extremes.Low,
                    IncludeLongitudinalReturns = true,
                    Planets = planetEnum.Value
                },
                new CandleStickMarkerSpecification
                {
                    Extremes = Extremes.High,
                    IncludeLongitudinalReturns = true,
                    Planets = planetEnum.Value
                }
            };

            return await ProcessSpecs(specs, symbol, order, limit);
        }


        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> ProcessSpecs(CandleStickMarkerSpecification spec, string symbol, int order, int? limit)
            => await ProcessSpecs(new[] { spec }, symbol, order, limit);

        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> ProcessSpecs(IEnumerable<CandleStickMarkerSpecification> specs, string symbol, int order, int? limit)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var data = await GetOrAddAllData(symbol);
            var ret = new List<CandleStickMarkerDto>();

            foreach (var spec in specs)
            {
                ret.AddRange(ExecuteSpec(spec, data, order, limit));
            }

            return Ok(ret.OrderBy(_ => _.Time));
        }

        private IEnumerable<CandleStickMarkerDto> ExecuteSpec(CandleStickMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
             => _candleStickMarkerCalculator.GetMarkers(spec, candles, order, limit).Select(_ => _.ToCandleStickMarkerDto());

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
