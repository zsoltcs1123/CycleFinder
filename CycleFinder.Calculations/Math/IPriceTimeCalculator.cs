using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Math
{
    public interface IPriceTimeCalculator
    {
        public double?[] ConvertLongitudesToPrices(double[] longitudes, double currentPrice, double increment);
        public IEnumerable<PriceLevel> GetPriceLevels(double maxValue, double increment, double minValue = 0);
        public bool AtHarmonicCrossing(double coordinate);
    }
}
