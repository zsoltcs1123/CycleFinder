using CycleFinder.Models.Candles;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class HighCandleMarker : EventMarker
    {
        public override MarkerPosition Position => MarkerPosition.AboveBar;

        public override MarkerShape Shape => MarkerShape.ArrowDown;

        public HighCandleMarker(CandleStick candle, Color color, int? id = null, int? turnId = null) : base(candle, color)
        {
            Text = turnId.HasValue ? $"TURN #{id}/{turnId}" : $"HIGH {(id == null ? "" : "#")}{id}";
        }
    }
}
