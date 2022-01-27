using CycleFinder.Calculations.Services.Ephemeris.Aspects;
using CycleFinder.Dtos;
using CycleFinder.Extensions;
using CycleFinder.Models;
using CycleFinder.Models.Extensions;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AstroEventDto>>> GetAspectsForPeriod(
            [FromQuery] long from,
            [FromQuery] long to)
        {
            return Ok((await _aspectCalculator.GetAspects(from.FromUnixTimeStamp(), to.FromUnixTimeStamp(), Planet.AllExceptMoon, AspectType.MainAspects))
                .Select(_ => _.ToAstroEventDto()).OrderBy(_ => _.Time));
        }
    }
}
