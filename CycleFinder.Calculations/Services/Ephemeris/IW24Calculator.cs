using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Services
{
    public interface IW24Calculator
    {
        public double?[] ConvertLongitudesToPrices(double[] longitudes, double currentPrice, double increment);
        public IEnumerable<W24PriceLevel> GetPriceLevels(double maxValue, double increment, double minValue = 0);
    }
}
