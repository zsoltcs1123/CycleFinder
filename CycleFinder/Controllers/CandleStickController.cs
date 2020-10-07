using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Models;
using CycleFinder.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CycleFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandleStickController : ControllerBase
    {
        private readonly ILogger<CandleStickController> _logger;
        private readonly IExternalDataService _dataService;

        public CandleStickController(ILogger<CandleStickController> logger, IExternalDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        [HttpGet("{symbol}")]
        public async Task<IEnumerable<CandleStick>> GetAllData(string symbol)
        {
            //TODO: change to ActionResult
            return await _dataService.GetAllData(symbol, TimeFrame.Daily);
        }
    }
}
