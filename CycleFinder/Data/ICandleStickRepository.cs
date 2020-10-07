using CycleFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public interface ICandleStickRepository
    {
        Task<List<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null);
        Task<List<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame);
    }
}
