using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Calculations.Services.Ephemeris;
using CycleFinder.Dtos;
using CycleFinder.Models.Extensions;
using CycleFinder.Services;
using Microsoft.AspNetCore.Mvc;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class PlanetaryLinesController : ControllerBase
    {
        private readonly IQueryParameterProcessor _parameterProcessor;
        private readonly IPlanetaryLinesCalculator _planetaryLinesCalculator;

        public PlanetaryLinesController(IQueryParameterProcessor parameterProcessor, IPlanetaryLinesCalculator planetaryLinesCalculator)
        {
            _parameterProcessor = parameterProcessor;
            _planetaryLinesCalculator = planetaryLinesCalculator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanetaryLinesDto>>> GetPlanetaryLines(
            [FromQuery] string planet, 
            [FromQuery] double currentPrice, 
            [FromQuery] long from, 
            [FromQuery] int upperOctaves = 1,
            [FromQuery] int lowerOctaves = 1)
        {
            var planetEnum = _parameterProcessor.PlanetFromString(planet);

            if (!planetEnum.HasValue)
            {
                return NotFound();
            }

            return Ok((await _planetaryLinesCalculator.GetPlanetaryLines(planetEnum.Value, currentPrice, DateTimeExtensions.FromUnixTimeStamp(from), upperOctaves, lowerOctaves))
                .Select(pLine => new PlanetaryLinesDto(pLine)));
        }
    }
}
