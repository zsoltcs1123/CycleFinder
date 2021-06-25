using System;
using System.Collections.Generic;

namespace CycleFinder.Models.Ephemeris
{
    public class RetrogradeCycles
    {
        public Planet Planet { get; }

        public Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus? RetrogradeStatus)> RetrogradeStatusByDays { get; } = new();

        public RetrogradeCycles(Planet planet, Dictionary<DateTime, (Coordinates Coordinates, RetrogradeStatus? RetrogradeStatus)> retrogradeStatusByDays)
        {
            Planet = planet;
            RetrogradeStatusByDays = retrogradeStatusByDays;
        }
    }
}
