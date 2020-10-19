using CycleFinder.Models.Candles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CycleFinder.Models.Markers
{
    public class LongitudinalReturnMarker : EventMarker
    {
        public LongitudinalReturnMarker(CandleStick candle, Color color, Planet planet, double longitude): base(candle, color)
        {
            Text = $"{planet.GetDescription()}: {longitude}";
        }
    }
}
