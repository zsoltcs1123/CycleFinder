using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Math
{
    public class PriceOctaveCalculator
    {
        public List<double> Octaves { get; }

        public PriceOctaveCalculator(double initialPrice, double increment, int lowerOctaves, int upperOctaves)
        {
            var keyNumber = increment * 24;

            var octaves = new List<double>();

            octaves.AddRange(Enumerable.Range(1, lowerOctaves).Select(_ => initialPrice - _ * keyNumber));
            octaves.Add(initialPrice);
            octaves.AddRange(Enumerable.Range(1, upperOctaves).Select(_ => initialPrice + _ * keyNumber));
            Octaves = octaves.Where(_ => _ > 0).ToList();
            Octaves.Sort();
        }
    }
}
