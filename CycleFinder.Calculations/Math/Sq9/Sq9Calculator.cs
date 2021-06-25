using CycleFinder.Calculations.Extensions;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Math.Sq9
{
    public class SQ9Calculator : ISq9Calculator
    {
        private static readonly double[] _cardinalCrosses;
        private static readonly double[] _fixedCrosses;

        static SQ9Calculator()
        {
            var crosses = GenerateCrosses();

            _cardinalCrosses = GetCardinalCrosses(crosses);
            _fixedCrosses = GetFixedCrosses(crosses);
        }

        private static Dictionary<string, double[]> GenerateCrosses(int rounds = 8)
        {
            int start = 1;
            return new Dictionary<string, double[]>()
            {
                {"w", GenerateVector(start + 1, rounds) },
                {"nw", GenerateVector(start + 2, rounds) },
                {"n", GenerateVector(start + 3, rounds) },
                {"ne", GenerateVector(start + 4, rounds) },
                {"e", GenerateVector(start + 5, rounds) },
                {"se", GenerateVector(start + 6, rounds) },
                {"s", GenerateVector(start + 7, rounds) },
                {"sw", GenerateVector(start + 8, rounds) },
            };
        }

        private static double[] GetCardinalCrosses(Dictionary<string, double[]> crosses)
        {
            return crosses["e"].Concat(crosses["w"]).Concat(crosses["s"]).Concat(crosses["n"]).ToArray();
        }

        private static double[] GetFixedCrosses(Dictionary<string, double[]> crosses)
        {
            return crosses["nw"].Concat(crosses["ne"]).Concat(crosses["sw"]).Concat(crosses["se"]).ToArray();
        }

        private static double[] GenerateVector(double start, int rounds = 8)
        {
            var lst = new List<double>
            {
                start
            };

            for (int i = 0; i < rounds; i++)
            {
                var next = System.Math.Pow(System.Math.Sqrt(lst[i]) + 2, 2).TruncateDecimals();
                if (next == 16)
                {
                    lst.Add(next - 1);
                }
                else
                {
                    lst.Add(next);
                }
            }
            return lst.ToArray();
        }


        public bool AtCardinalCrossing(double longitude)
        {
            return _cardinalCrosses.Contains(longitude.TruncateDecimals());
        }

        public bool AtFixedCrossing(double longitude)
        {
            return _fixedCrosses.Contains(longitude.TruncateDecimals());
        }

        public IEnumerable<PriceLevel> GetPriceLevels(double maxValue, int multiplier, double minValue = 0)
        {
            //TODO figure out some way to check if multiplier is power of 10 (negative or positive)

            double octaves = maxValue / (360 * multiplier);
            int rounds = (octaves.TruncateIntegerPart() != 0 ? octaves.TruncateDecimals() + 1 : octaves.TruncateDecimals()) * 8;

            //Todo filter out minvalue
            var crosses = GenerateCrosses(rounds);

            var cardinals = GetCardinalCrosses(crosses).Select(_ => _ * multiplier);
            var fix = GetFixedCrosses(crosses).Select(_ => _ * multiplier);

            return cardinals
                .Select(_ => new PriceLevel(_, Models.W24LineType._24Line))
                .Concat(fix.Select(_ => new PriceLevel(_, Models.W24LineType.IntermediateLine)))
                .OrderBy(level => level.Value);
        }
    }
}
