using CycleFinder.Models.Candles;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    public class LowCandleMarker : CandleMarkerBase
    {
        public override MarkerPositions Position => MarkerPositions.BelowBar;

        public override MarkerShapes Shape => MarkerShapes.ArrowUp;

        public LowCandleMarker(CandleStick candle, Color color, int? id = null, int? turnId = null)
        {
            Candle = candle;
            Color = color;
            Text = turnId.HasValue ? $"TURN #{id}/{turnId}" : $"LOW {(id == null ? "" : "#")}{id}";

        }
    }
}
