using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public class CandleStickMarkerSpecification
    {
        public Extremes Extremes { get; set; }
        public bool IncluePrimaryStaticCycles { get; set; }
        public bool IncludeSecondaryStaticCycles { get; set; }
        public bool IncludeLongitudinalReturns { get; set; }
        public bool IncludeRetrogrades { get; set; }
        public Planets Planets { get; set; }
        public Ephemeris Ephemeris { get; set; }
        public bool IncludeNone => !IncluePrimaryStaticCycles && !IncludeSecondaryStaticCycles && !IncludeLongitudinalReturns && !IncludeRetrogrades;

    }
}
