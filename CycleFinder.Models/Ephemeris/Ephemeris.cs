using System;
using System.Collections.Generic;

namespace CycleFinder.Models.Ephemeris
{
    public class Ephemeris
    {
        IDictionary<DateTime, Coordinates> Coordinates { get; }
        public Planets Planet { get; }

        public Ephemeris(IDictionary<DateTime, Coordinates> coordinates, Planets planet)
        {
            Coordinates = coordinates;
            Planet = planet;
        }
    }
}
