using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Calculations.Services.Ephemeris;
using CycleFinder.Dtos;
using CycleFinder.Models;
using CycleFinder.Models.Extensions;
using CycleFinder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class PlanetaryLinesController : ControllerBase
    {
        private readonly IQueryParameterProcessor _parameterProcessor;
        private readonly IPlanetaryLineCalculator _planetaryLineCalculator;

        public PlanetaryLinesController(IQueryParameterProcessor parameterProcessor, IPlanetaryLineCalculator planetaryLineCalculator)
        {
            _parameterProcessor = parameterProcessor;
            _planetaryLineCalculator = planetaryLineCalculator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanetaryLineDto>>> GetPlanetaryLines(
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

            return Ok((await _planetaryLineCalculator.GetPlanetaryLines(planetEnum.Value, currentPrice, DateTimeExtensions.FromUnixTimeStamp(from), upperOctaves, lowerOctaves))
                .Select(pLine => new PlanetaryLineDto(pLine)));
        }
    }
}
