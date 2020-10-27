using System;
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

            var fromDate = DateTimeExtensions.FromUnixTimeStamp(from);
            var toDate = DateTime.UtcNow.AddYears(1); //Default 1 years into the future. maybe make this a query param

            //for su,me,ve,ma it makes no sense to display more than one year back since the

            return Ok((await _planetaryLinesCalculator.GetPlanetaryLines(planetEnum.Value, currentPrice, fromDate, toDate, upperOctaves, lowerOctaves))
                .Select(pLine => new PlanetaryLinesDto(pLine)));
        }
    }
}
