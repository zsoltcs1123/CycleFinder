using CycleFinder.Models;
using CycleFinder.Models.Candles;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Math.Extremes
{
    public interface ILocalExtremeCalculator
    {
        IEnumerable<CandleStick> GetLocalMinima(IEnumerable<CandleStick> data, int order);
        IEnumerable<CandleStick> GetLocalMaxima(IEnumerable<CandleStick> data, int order);
        IEnumerable<CandleStick> GetLocalExtremes(IEnumerable<CandleStick> data, int order);
    }
}
