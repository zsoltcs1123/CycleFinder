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
using System;
using CycleFinder.Models.Extensions;
using CycleFinder.Services;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class CandleStickMarkerController : CandleStickController
    {
        private readonly ICandleStickMarkerCalculator _candleStickMarkerCalculator;
        private readonly IQueryParameterProcessor _parameterProcessor;

        public CandleStickMarkerController(
            ILogger<CandleStickController> logger,
            ICandleStickRepository candleStickRepository,
            IAppCache cache,
            ICandleStickMarkerCalculator candleStickMarkerCalculator,
            IQueryParameterProcessor queryParameterProcessor) : base(logger, candleStickRepository, cache)
        {
            _candleStickMarkerCalculator = candleStickMarkerCalculator;
            _parameterProcessor = queryParameterProcessor;
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
            var planetEnum = _parameterProcessor.PlanetsFromString(planet);

            if (planetEnum == Planet.None)
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
            var planetEnum = _parameterProcessor.PlanetsFromString(planet);

            if (planetEnum == Planet.None)
            {
                return NotFound();
            }

            var spec = new ExtremeCandleWithPlanetsMarkerSpecification
            {
                Extreme = Extreme.High,
                IncludeLongitudinalReturns = false,
                Planets = planetEnum,
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
            var planetEnum = _parameterProcessor.PlanetsFromString(planet);

            if (planetEnum == Planet.None)
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> GetAspects(
            [FromQuery] long from,
            [FromQuery] string planet,
            [FromQuery] string aspect)
        {
            //Enum.HasFlag always true for None (0) 
            var planets = _parameterProcessor.PlanetsFromString(planet).GetFlags().Where(_ => _ != Planet.None).ToList();
            var aspectTypes = AspectTypesFromString(aspect);

            if (planets.Count() != 2)
            {
                return NotFound();
            }

            var spec = new AspectMarkerSpecification(DateTimeExtensions.FromUnixTimeStamp(from), planets[0], planets[1], aspectTypes);

            return Ok((await _candleStickMarkerCalculator.GetMarkers(spec)).Select(_ => _.ToCandleStickMarkerDto()));
        }


        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> ProcessSpecs(MarkerSpecification spec, string symbol, int order, int? limit)
            => await ProcessSpecs(new[] { spec }, symbol, order, limit);

        private async Task<ActionResult<IEnumerable<CandleStickMarkerDto>>> ProcessSpecs(IEnumerable<MarkerSpecification> specs, string symbol, int order, int? limit)
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

        private async Task<IEnumerable<CandleStickMarkerDto>> ExecuteSpec(MarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
             => (await _candleStickMarkerCalculator.GetMarkers(spec, candles, order, limit)).Select(_ => _.ToCandleStickMarkerDto());

        private static AspectType AspectTypesFromString(string planet)
        {
            if (String.IsNullOrEmpty(planet)) return AspectType.All;

            AspectType ret = AspectType.None;

            foreach (string s in planet.Split(","))
            {
                var planetEnum = AspectTypeFromString(s);
                if (planetEnum.HasValue)
                {
                    ret |= planetEnum.Value;
                }
            }
            return ret;
        }

        private static AspectType? AspectTypeFromString(string aspect) => aspect switch
        {
            "cj" => AspectType.Conjunction,
            "op" => AspectType.Opposition,
            "sex" => AspectType.Sextile,
            "tri" => AspectType.Trine,
            "sq" => AspectType.Square,
            "ssex" => AspectType.SemiSextile,
            "icj" => AspectType.Inconjunct,
            _ => null,
        };

        private bool CheckPlanetExists(Planet? planet) => planet.HasValue;
    }
}
