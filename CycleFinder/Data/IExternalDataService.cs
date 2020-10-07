﻿using CycleFinder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Data 
{ 
    public interface IExternalDataService
    {
        Task<List<Symbol>> ListSymbols();
        Task<List<CandleStick>> GetData(string symbol, TimeFrame timeFrame, DateTime? startTime = null, DateTime? endTime = null);
        Task<List<CandleStick>> GetAllData(string symbol, TimeFrame timeFrame);
    }
}