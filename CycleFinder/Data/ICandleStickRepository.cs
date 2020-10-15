using CycleFinder.Models;
using CycleFinder.Models.Candles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public interface ICandleStickRepository
    {
        Task<IEnumerable<Symbol>> ListSymbols();
        Task<IEnumerable<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null);
        Task<IEnumerable<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame);
    }
}
