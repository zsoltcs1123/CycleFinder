using CycleFinder.Models.Candles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CycleFinder.Models.Markers
{
    public interface ICandleStickMarker
    {
        public CandleStick Candle { get; }
        public Color Color { get; }
        public string Text { get; }
        public MarkerPositions Position { get; }
        public MarkerShapes Shape { get; }
    }
}
