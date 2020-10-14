using CycleFinder.Models.Candles;
using System.Collections.Generic;
using System.Drawing;

namespace CycleFinder.Models.Markers
{
    class CandleWithPlanetInfoMarker : CandleMarkerBase
    {
        public override MarkerPositions Position => MarkerPositions.AboveBar;

        public override MarkerShapes Shape => MarkerShapes.Circle;

        public CandleWithPlanetInfoMarker(CandleStick candle, Color color, IDictionary<Planets, double> planetInfos)
        {
            Candle = candle;
            Color = color;
        }
    }
}
