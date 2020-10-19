using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Text;

namespace CycleFinder.Models.Candles
{
    public static class CandleStickExtensions
    {
        public static CandleStick AddEphemerisEntry(this CandleStick candle, EphemerisEntry entry)
        {
            candle.EphemerisEntry = entry;
            return candle;
        }
    }
}
