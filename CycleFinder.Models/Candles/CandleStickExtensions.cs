using CycleFinder.Models.Ephemeris;

namespace CycleFinder.Models.Candles
{
    public static class CandleStickExtensions
    {
        public static CandleStick AddEphemerisEntry(this CandleStick candle, EphemerisEntry entry)
        {
            candle.SetEmphemerisEntry(entry);
            return candle;
        }
    }
}
