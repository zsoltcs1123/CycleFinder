using CycleFinder.Dtos;
using CycleFinder.Models;
using System.Drawing;

namespace CycleFinder.Extensions
{
    public static class MapperExtensions
    {
        public static CandleStickDto ToCandleStickDto(this CandleStick candleStick)
        {
            return new CandleStickDto(candleStick.TimeInSeconds, candleStick.Open, candleStick.High, candleStick.Low, candleStick.Close, candleStick.Volume);
        }

        public static LowCandleStickDto ToLowCandleStickDto(this CandleStick candleStick, Color color)
        {
            return new LowCandleStickDto(candleStick.TimeInSeconds, color);
        }
    }
}
