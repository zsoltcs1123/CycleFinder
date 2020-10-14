using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public class CandleStickMarkerSpecification
    {
        public IEnumerable<CandleStick> Candles { get; }
        public Extremes Extremes { get; }
        public IRandomColorGenerator RandomColorGenerator {get;}

        public bool IncluePrimaryStaticCycles { get; set; }
        public bool IncludeSecondaryStaticCycles { get; set; }
        public bool IncludeLongitudinalReturns { get; set; }
        public bool IncludeRetrogrades { get; set; }
        public Planets Planets { get; set; }
        public Ephemeris Ephemeris { get; set; }
        public bool IncludeNone => !IncluePrimaryStaticCycles && !IncludeSecondaryStaticCycles && !IncludeLongitudinalReturns && !IncludeRetrogrades;

        public CandleStickMarkerSpecification(
            IEnumerable<CandleStick> candles, 
            Extremes extreme,
            IRandomColorGenerator randomColorGenerator)
        {
            Candles = candles;
            Extremes = extreme;
        }
    }
}
