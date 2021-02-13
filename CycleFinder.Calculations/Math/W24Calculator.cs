using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System.Collections.Generic;
using System.Linq;
using CycleFinder.Calculations.Extensions;

namespace CycleFinder.Calculations.Math
{
    public class W24Calculator : IW24Calculator
    {
        public double?[] ConvertLongitudesToPrices(double[] longitudes, double currentPrice, double increment)
        {
            double keyNumber = increment * 24;
            int initialOctave = (currentPrice / keyNumber).TruncateDecimals();

            double currentTimeRatio, truncatedCurrentTimeRatio, currentOctave, previousTimeRatio;

            var ret = new List<double?>();
            currentOctave = initialOctave;

            for (int i = 0; i < longitudes.Length; i++)
            {
                currentTimeRatio = GetTimeRatio(longitudes[i]);
                truncatedCurrentTimeRatio = currentTimeRatio.TruncateIntegerPart();

                if (i > 1)
                {
                    previousTimeRatio = GetTimeRatio(longitudes[i - 1]);
                    var octaveShift = previousTimeRatio.TruncateDecimals() - currentTimeRatio.TruncateDecimals();
                    if (octaveShift == -1 || octaveShift == 14)
                    {
                        currentOctave++;
                    }
                    else if (octaveShift == 1 || octaveShift == -14)
                    {
                        currentOctave--;
                    }
                }

                double finalPrice = (currentOctave + truncatedCurrentTimeRatio) * keyNumber;

                if (finalPrice > 0)
                {
                    ret.Add(finalPrice);
                }
                else ret.Add(null);
            }
            return ret.ToArray();
        }

        public IEnumerable<PriceLevel> GetPriceLevels(double maxValue, double increment, double minValue = 0)
        {
            double keyNumber = increment * 24;

            int maxOctave = (maxValue / keyNumber).TruncateDecimals();
            int minOctave = (minValue / keyNumber).TruncateDecimals();

            var _24lines = Enumerable.Range(minOctave, maxOctave).Select(n => new PriceLevel(n*keyNumber, W24LineType._24Line)).ToList();

            var imLines = _24lines.Select(line => new []
            {
                new PriceLevel(line.Value + (keyNumber * 0.25), W24LineType.IntermediateLine),
                new PriceLevel(line.Value + (keyNumber * 0.5), W24LineType.IntermediateLine),
                new PriceLevel(line.Value + (keyNumber * 0.75), W24LineType.IntermediateLine) 
            });

            return _24lines.Concat(imLines.SelectMany(line => line));
        }

        public bool AtW24Crossing(double coordinate)
        {
            return System.Math.Round(coordinate / 24, 1) % 1 == 0;
        }

        private static double GetTimeRatio(double longitude) => longitude / 24;
    }
}
