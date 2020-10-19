using System;
using System.Linq;
using CycleFinder.Models.Ephemeris;

namespace CycleFinder.Models.Specifications
{
    public class ExtremeCandleWithPlanetsMarkerSpecification : CandleMarkerSpecification
    {
        public Extreme Extreme { get; set; }
        public bool IncludeLongitudinalReturns { get; set; }
        public Planet Planets { get; set; }

        public override bool IsValid => Extreme == Extreme.Low || Extreme == Extreme.High;
    }
}
