using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CycleFinder.Models.Markers
{
    public class LowCandleMarker : EventMarker
    {
        public override MarkerPosition Position => MarkerPosition.BelowBar;

        public override MarkerShape Shape => MarkerShape.ArrowUp;

        public LowCandleMarker(CandleStick candle, Color color, int? id = null, int? turnId = null) : base(candle, color)
        {
            Text = turnId.HasValue ? $"TURN #{id}/{turnId}" : $"LOW {(id == null ? "" : "#")}{id}";

        }

        public LowCandleMarker(CandleStick candle, Color color, IDictionary<Planet, Coordinates> coordinates) : base(candle, color) 
        {
            var nl = Environment.NewLine;
            StringBuilder sb = new StringBuilder($"LOW{nl}");

            foreach (var kvp in coordinates)
            {
                sb.Append($"{kvp.Key}: {kvp.Value.Longitude}{nl}");
            }

            Text = sb.ToString();
        }
    }
}
