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
            MarkerPosition markerPosition = MarkerPosition.BelowBar,
            MarkerShape markerShape = MarkerShape.ArrowUp)
        {
            return new CandleStickMarkerDto(candleStick, color, text, markerPosition, markerShape);
        }

        public static CandleStickMarkerDto ToLowMarkerDto(this CandleStick candle, Color color, int? id = null)
            => candle.ToCandleStickMarkerDto(color, $"LOW {(id == null ? "" : "#")}{id}", MarkerPosition.BelowBar, MarkerShape.ArrowUp);

        public static CandleStickMarkerDto ToHighMarkerDto(this CandleStick candle, Color color, int? id = null)
            => candle.ToCandleStickMarkerDto(color, $"HIGH {(id == null ? "" : "#")}{id}", MarkerPosition.AboveBar, MarkerShape.ArrowDown);

        public static CandleStickMarkerDto ToHighTurnMarkerDto(this CandleStick candle, Color color, int lowId, int turnId)
            => candle.ToCandleStickMarkerDto(color, $"TURN #{lowId}/{turnId}", MarkerPosition.AboveBar, MarkerShape.ArrowDown);

        public static CandleStickMarkerDto ToLowTurnMarkerDto(this CandleStick candle, Color color, int highId, int turnId)
            => candle.ToCandleStickMarkerDto(color, $"TURN #{highId}/{turnId}", MarkerPosition.BelowBar, MarkerShape.ArrowUp);

        public static CandleStickMarkerDto ToPlanetPositionMarkerDto(this CandleStick candle, Color color, Planet planet, double longitude)
            => candle.ToCandleStickMarkerDto(color, $"{planet.GetDescription()}:{longitude}", MarkerPosition.BelowBar, MarkerShape.ArrowUp);
    }
}
