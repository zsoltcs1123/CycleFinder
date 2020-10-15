using CycleFinder.Models.Candles;
using System.Drawing;

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
    }
}
