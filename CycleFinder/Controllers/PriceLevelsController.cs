using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Calculations.Services;
using CycleFinder.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CycleFinder.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]", Name = "[controller]_[action]")]
    public class PriceLevelsController : Controller
    {
        private readonly IW24Calculator _w24Calculator;

        public PriceLevelsController(IW24Calculator w24Calculator)
        {
            _w24Calculator = w24Calculator;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceLevelDto>>> GetW24PriceLevels(
            [FromQuery] double maxValue,
            [FromQuery] double increment,
            [FromQuery] double minValue = 0)
        {
            return Ok(await Task.Run(() => _w24Calculator.GetPriceLevels(maxValue, increment, minValue).Select(_ => _.ToPriceLevelDto())));
        }
    }
}



