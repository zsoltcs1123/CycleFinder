using CycleFinder.Calculations.Services.Ephemeris.Aspects;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Models;
using CycleFinder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class AstroEventController : ControllerBase
    {
        private readonly ILogger<AstroEventController> _logger;
        private readonly IQueryParameterProcessor _parameterProcessor;
        private readonly IAspectCalculator _aspectCalculator;

        public AstroEventController(ILogger<AstroEventController> logger, IQueryParameterProcessor parameterProcessor, IAspectCalculator aspectCalculator)
        {
            _logger = logger;
            _parameterProcessor = parameterProcessor;
            _aspectCalculator = aspectCalculator;
        }

        /// <summary>
        /// Gets any aspects for any planets in a given period.
        /// </summary>
        /// <param name="from">Start time of the period</param>
        /// <param name="to">End time of the period</param>
        /// <param name="planet">Comma separated list of planets. Valide value are: mo, su, me, ve, ma, ju, sa, ur, ne, pl.
        /// If not specified, all planets are returned, except for Moon.</param>
        /// <param name="aspectType">Comma separated list of aspect types. Valid values are: cj, op, sq, tri, sex, ssex, icj.
        /// If not specified, the main aspects (Conjunction, Opposition, Square, Trine, Sextile) will be returned.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstroEventDto>>> GetAspects(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            [FromQuery] string planet = null,
            [FromQuery] string aspectType = null)
        {
            if (to < from)
            {
                throw new Exception("End date must be later than start date");
            }

            var planetEnum = _parameterProcessor.PlanetsFromString(planet);

            if (planetEnum == Planet.None)
            {
                throw new Exception("No planets specified");
            }


            var aspectEnum = _parameterProcessor.AscpectTypesFromString(aspectType);

            if (aspectEnum == AspectType.None)
            {
                throw new Exception("No aspects specified");
            }

            return Ok((await _aspectCalculator.GetAspects(from, to, planetEnum, aspectEnum)).Select(_ => _.ToAstroEventDto()).OrderBy(_ => _.Time));
        }
    }
}
