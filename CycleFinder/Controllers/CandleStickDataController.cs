using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Models;
using CycleFinder.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CycleFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandleStickDataController : ControllerBase
    {
        private readonly ILogger<CandleStickDataController> _logger;
        private readonly IDataService _dataService;

        public CandleStickDataController(ILogger<CandleStickDataController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        [HttpGet("{symbol}")]
        public async Task<IEnumerable<CandleStickData>> GetAllData(string symbol)
        {
            //TODO: change to ActionResult
            return await _dataService.GetAllData(symbol, TimeFrame.Daily);
        }
    }
}
