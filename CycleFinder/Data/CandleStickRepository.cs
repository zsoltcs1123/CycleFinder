using CycleFinder.Controllers;
using CycleFinder.Extensions;
using CycleFinder.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public class CandleStickRepository : ICandleStickRepository
    {
        private readonly ILogger<CandleStickRepository> _logger;
        private readonly IExternalDataService _externalDataService;

        public CandleStickRepository(ILogger<CandleStickRepository> logger, IExternalDataService externalDataService)
        {
            _logger = logger;
            _externalDataService = externalDataService;
        }

        public Task<List<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame)
        {
            _logger.LogInformation($"Calling external data service with parameters [{nameof(symbol)}: {symbol}, {nameof(timeFrame)}: {timeFrame.GetDescription()}]");
            return _externalDataService.GetAllData(symbol, timeFrame);
        }

        public Task<List<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null)
        {
            _logger.LogInformation($"Calling external data service with parameters [{nameof(symbol)}: {symbol}, {nameof(timeFrame)}: {timeFrame.GetDescription()}," +
                $"{nameof(startTime)}: {startTime}, {nameof(endTime)}: {endTime}");
            return _externalDataService.GetData(symbol, timeFrame, startTime, endTime);
        }
    }
}
