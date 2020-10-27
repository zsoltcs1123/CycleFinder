using System;
using System.Collections.Generic;

namespace CycleFinder.Models.Ephemeris
{
    public class PlanetaryLine
    {
        public Planet Planet { get; }
        public IEnumerable<(DateTime Time, double Value)> Values { get; }

        public PlanetaryLine(Planet planet, IEnumerable<(DateTime Time, double Value)> values)
        {
            Planet = planet;
            Values = values;
        }
    }
}
