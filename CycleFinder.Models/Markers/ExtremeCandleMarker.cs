using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CycleFinder.Models.Markers
{
    public class ExtremeCandleMarker : EventMarker
    {
        private Extreme _type;

        public override MarkerPosition Position => _type == Extreme.High ? MarkerPosition.AboveBar : MarkerPosition.BelowBar;

        public override MarkerShape Shape => _type == Extreme.High ? MarkerShape.ArrowDown : MarkerShape.ArrowUp;

        public ExtremeCandleMarker(CandleStick candle, Extreme type, Color color, int? id = null, int? turnId = null) : base(candle, color)
        {
            _type = type;
            Text = turnId.HasValue ? $"TURN #{id}/{turnId}" : $"{(_type == Extreme.High ? "HIGH" : "LOW")} {(id == null ? "" : "#")}{id}";
        }

        public ExtremeCandleMarker(CandleStick candle, Extreme type, Color color, IDictionary<Planet, Coordinates> planetaryCoordinates) : base(candle, color)
        {
            _type = type;

            var nl = Environment.NewLine;
            StringBuilder sb = new StringBuilder($"{(_type == Extreme.High ? "HIGH" : "LOW")}{nl}");

            foreach (var coordinate in planetaryCoordinates)
            {
                sb.Append($"{coordinate.Key}: {coordinate.Value.Longitude}{nl}");
            }

            Text = sb.ToString();
        }
    }
}
