using CycleFinder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Services 
{ 
    public interface IDataService
    {
        Task<List<Symbol>> ListSymbols();
        Task<List<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null);
        Task<List<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame);
    }
}
