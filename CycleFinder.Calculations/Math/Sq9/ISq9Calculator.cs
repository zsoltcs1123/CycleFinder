using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Math.Sq9
{
    public interface ISq9Calculator
    {
        public bool AtCardinalCrossing(double longitude);
        public bool AtFixedCrossing(double longitude);
        public IEnumerable<PriceLevel> GetPriceLevels(double maxValue, int multiplier, double minValue = 0);
    }
}
