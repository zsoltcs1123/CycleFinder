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
using CycleFinder.Models.Specifications;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickMarkerController : CandleStickController
    {
        private readonly ICandleStickMarkerCalculator _candleStickMarkerCalculator;

        public CandleStickMarkerController(
            ILogger<CandleStickController> logger,
            ICandleStickRepository candleStickRepository,
            IAppCache cache,
            ICandleStickMarkerCalculator candleStickMarkerCalculator) : base(logger, candleStickRepository, cache)
        {
            _candleStickMarkerCalculator = candleStickMarkerCalculator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLows([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var spec = new ExtremeCandleMarkerSpecification
            {
                Extreme = Extreme.Low
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighs([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {

            var spec = new ExtremeCandleMarkerSpecification
            {
                Extreme = Extreme.High
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetExtremes([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var specs = new[]
            {
                new ExtremeCandleMarkerSpecification
                {
                    Extreme = Extreme.Low
                },
                new ExtremeCandleMarkerSpecification
                {
                    Extreme = Extreme.High
                }
            };

            return await ProcessSpecs(specs, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetLowsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var spec = new ExtremeCandleWithTurnsMarkerSpecification
            {
                Extreme = Extreme.Low,
                IncluePrimaryStaticCycles = true
            };

            return await ProcessSpecs(spec, symbol, order, limit);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetHighsWithTurns([FromQuery] string symbol, [FromQuery] int order = 15, [FromQuery] int? limit = null)
        {
            var spec = new ExtremeCandleWithTurnsMarkerSpecification
            {
                Extreme = Extreme.High,
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

            var spec = new ExtremeCandleWithPlanetsMarkerSpecification
            {
                Extreme = Extreme.Low,
                IncludeLongitudinalReturns = false,
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

            var spec = new ExtremeCandleWithPlanetsMarkerSpecification
            {
                Extreme = Extreme.High,
                IncludeLongitudinalReturns = false,
                Planets = PlanetFromString(planet) ?? Planet.All,
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
                new ExtremeCandleWithPlanetsMarkerSpecification
                {
                    Extreme = Extreme.Low,
                    IncludeLongitudinalReturns = true,
                },
                new ExtremeCandleWithPlanetsMarkerSpecification
                {
                    Extreme = Extreme.High,
                    IncludeLongitudinalReturns = true,
                }
            };

            return await ProcessSpecs(specs, symbol, order, limit);
        }


        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> ProcessSpecs(CandleMarkerSpecification spec, string symbol, int order, int? limit)
            => await ProcessSpecs(new[] { spec }, symbol, order, limit);

        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> ProcessSpecs(IEnumerable<CandleMarkerSpecification> specs, string symbol, int order, int? limit)
        {
            if (!CheckSymbolExists(symbol))
            {
                return NotFound();
            }

            var data = await GetOrAddAllData(symbol);
            var ret = new List<CandleStickMarkerDto>();

            foreach (var spec in specs)
            {
                ret.AddRange(await ExecuteSpec(spec, data, order, limit));
            }

            return Ok(ret.OrderBy(_ => _.Time));
        }

        private async Task<IEnumerable<CandleStickMarkerDto>> ExecuteSpec(CandleMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
             => (await _candleStickMarkerCalculator.GetMarkers(spec, candles, order, limit)).Select(_ => _.ToCandleStickMarkerDto());

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
