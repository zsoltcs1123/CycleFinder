using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Calculations.Ephemeris.PlanetaryLines;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Models;
using CycleFinder.Models.Extensions;
using CycleFinder.Models.Markers;
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

        private bool CheckTimeFrameExists(string timeFrameStr, out TimeFrame? timeFrame)
        {
            var tf = _parameterProcessor.TimeFrameFromString(timeFrameStr);

            timeFrame = tf ?? null;

            return tf.HasValue;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanetaryLinesDto>>> GetPlanetaryLines(
            [FromQuery] string planet, 
            [FromQuery] double currentPrice, 
            [FromQuery] long from, 
            [FromQuery] string timeFrame,
            [FromQuery] double increment,
            [FromQuery] int upperOctaves = 1,
            [FromQuery] int lowerOctaves = 1)
        {
            var planetEnum = _parameterProcessor.PlanetFromString(planet);

            if (!planetEnum.HasValue)
            {
                return NotFound();
            }

            if (!CheckTimeFrameExists(timeFrame, out TimeFrame? tf))
            {
                return NotFound();
            }

            var fromDate = DateTimeExtensions.FromUnixTimeStamp(from);
            var toDate = DateTime.UtcNow.AddDays(30); //Default 1 years into the future. maybe make this a query param

            return Ok((await _planetaryLinesCalculator.GetPlanetaryLines(planetEnum.Value, currentPrice, fromDate, toDate, tf.Value, increment, upperOctaves, lowerOctaves))
                .Select(pLine => new PlanetaryLinesDto(pLine)));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanetaryLinesDto>>> GetW24Crossings(
            [FromQuery] string planet,
            [FromQuery] long from)
        {
            var planetEnum = _parameterProcessor.PlanetFromString(planet);

            if (!planetEnum.HasValue)
            {
                return NotFound();
            }

            var fromDate = DateTimeExtensions.FromUnixTimeStamp(from);

            return Ok((await _planetaryLinesCalculator.GetW24Crossings(planetEnum.Value, fromDate))
                .Select(cross => new W24CrossingMarker(cross.Time, cross.Planet, cross.Position))
                .Select(marker => marker.ToCandleStickMarkerDto()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanetaryLinesDto>>> GetSQ9Crossings(
            [FromQuery] string planet,
            [FromQuery] long from)
        {
            var planetEnum = _parameterProcessor.PlanetFromString(planet);

            if (!planetEnum.HasValue)
            {
                return NotFound();
            }

            var fromDate = DateTimeExtensions.FromUnixTimeStamp(from);

            return Ok((await _planetaryLinesCalculator.GetSQ9Crossings(planetEnum.Value, fromDate))
                .Select(cross => new W24CrossingMarker(cross.Time, cross.Planet, cross.Position))
                .Select(marker => marker.ToCandleStickMarkerDto()));
        }


    }
}
