using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Math
{
    public interface IW24Calculator
    {
        public double?[] ConvertLongitudesToPrices(double[] longitudes, double currentPrice, double increment);
        public IEnumerable<PriceLevel> GetPriceLevels(double maxValue, double increment, double minValue = 0);
        public bool AtW24Crossing(double coordinate);
    }
}
