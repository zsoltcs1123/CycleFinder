using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Calculations.Math;
using CycleFinder.Calculations.Math.Sq9;
using CycleFinder.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class PriceLevelsController : Controller
    {
        private readonly IPriceTimeCalculator _w24Calculator;
        private readonly ISq9Calculator _sq9Calculator;

        public PriceLevelsController(IPriceTimeCalculator w24Calculator, ISq9Calculator sq9Calculator)
        {
            _w24Calculator = w24Calculator;
            _sq9Calculator = sq9Calculator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceLevelDto>>> GetW24PriceLevels(
            [FromQuery] double maxValue,
            [FromQuery] double increment,
            [FromQuery] double minValue = 0)
        {
            return Ok(await Task.Run(() => _w24Calculator.GetPriceLevels(maxValue, increment, minValue).Select(_ => _.ToPriceLevelDto())));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceLevelDto>>> GetSQ9PriceLevels(
            [FromQuery] double maxValue,
            [FromQuery] int multiplier,
            [FromQuery] double minValue = 1)
        {
            return Ok(await Task.Run(() => _sq9Calculator.GetPriceLevels(maxValue, multiplier, minValue).Select(_ => _.ToPriceLevelDto())));
        }
    }
}



