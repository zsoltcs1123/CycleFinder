using CycleFinder.Models;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public interface ICandleStickCalculator
    {
        IEnumerable<CandleStick> GetLocalMinima(IEnumerable<CandleStick> data, int order);
        IEnumerable<CandleStick> GetLocalMaxima(IEnumerable<CandleStick> data, int order);
        IEnumerable<CandleWithTurns> GetPrimaryTimeCyclesFromLows(IEnumerable<CandleStick> data, int order);
        IEnumerable<CandleWithTurns> GetPrimaryTimeCyclesFromHighs(IEnumerable<CandleStick> data, int order);
    }
}
