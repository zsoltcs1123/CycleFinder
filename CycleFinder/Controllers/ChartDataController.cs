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
    public class ChartDataController : ControllerBase
    {
        private readonly ILogger<ChartDataController> _logger;
        private readonly IDataService _dataService;

        public ChartDataController(ILogger<ChartDataController> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        [HttpGet("{symbol}")]
        public async Task<IEnumerable<CandleStick>> GetAllData(string symbol)
        {
            return await _dataService.GetAllData(symbol, TimeFrame.Daily);
        }
    }
}
