using CycleFinder.Dtos;
using CycleFinder.Models;

namespace CycleFinder.Extensions
{
    public static class MapperExtensions
    {
        public static CandleStickDto ToDto(this CandleStick candleStick)
        {
            return new CandleStickDto(candleStick.TimeInSeconds, candleStick.Open, candleStick.High, candleStick.Low, candleStick.Close, candleStick.Volume);
        }
    }
}
