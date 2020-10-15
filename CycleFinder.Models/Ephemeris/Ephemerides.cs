using System;
using System.Collections.Generic;

namespace CycleFinder.Models.Ephemeris
{
    public class Ephemerides
    {
        public Dictionary<DateTime, Dictionary<Planet ,Coordinates>> Coordinates { get; }

        public Ephemerides(Dictionary<DateTime, Dictionary<Planet, Coordinates>> coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
