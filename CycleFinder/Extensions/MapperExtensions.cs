using CycleFinder.Dtos;
using CycleFinder.Models;
using System;
using System.Drawing;

namespace CycleFinder.Extensions
{
    public static class MapperExtensions
    {
        public static CandleStickDto ToCandleStickDto(this CandleStick candleStick)
        {
            return new CandleStickDto(candleStick.TimeInSeconds, candleStick.Open, candleStick.High, candleStick.Low, candleStick.Close, candleStick.Volume);
        }

        public static CandleStickMarkerDto ToCandleStickMarkerDto(
            this CandleStick candleStick, 
            Color color,
            string text = "",
            MarkerPosition markerPosition = MarkerPosition.BelowBar)
        {
            return new CandleStickMarkerDto(candleStick.TimeInSeconds, color, text, markerPosition);
        }
    }
}
