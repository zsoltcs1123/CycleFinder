using CycleFinder.Models.Candles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CycleFinder.Models.Markers
{
    public abstract class CandleMarkerBase : ICandleStickMarker
    {
        public CandleStick Candle { get; protected set; }

        public Color Color { get; protected set; }

        public string Text { get; protected set; }

        public virtual MarkerPositions Position { get; }

        public virtual MarkerShapes Shape { get; }

    }
}
