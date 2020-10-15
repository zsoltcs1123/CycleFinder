using System;
using System.Collections.Generic;
using System.Text;
using CycleFinder.Models.Ephemeris;

namespace CycleFinder.Models.Specifications
{
    public class ExtremeCandleWithPlanetMarkerSpecification : CandleMarkerSpecification
    {
        public Extreme Extreme { get; set; }
        public bool IncludeLongitudinalReturns { get; set; }
        public Ephemeris.Ephemerides Ephemeris { get; set; }

        public override bool IsValid => throw new NotImplementedException();
    }
}
