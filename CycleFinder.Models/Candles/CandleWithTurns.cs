using System.Collections.Generic;

namespace CycleFinder.Models.Candles
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
