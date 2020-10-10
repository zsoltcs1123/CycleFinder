using CycleFinder.Models;
using System.Collections.Generic;

namespace CycleFinder.Calculations
{
    public class CandleWithTurns
    {
        public CandleStick Candle { get; }
        public IEnumerable<CandleStick> Turns { get; }

        public CandleWithTurns(CandleStick candle, IEnumerable<CandleStick> turns)
        {
            Candle = candle;
            Turns = turns;
        }
    }
}
