using CycleFinder.Calculations.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Math
{
    public class SQ9Calculator : ISQ9Calculator
    {
        private static readonly double[] _cardinalCrosses;
        private static readonly double[] _fixedCrosses;

        static SQ9Calculator()
        {
            var start = 1;
            var westVector = GenerateVector(start + 1 , 8);
            var nwVector = GenerateVector(start + 2 , 8);
            var northVector = GenerateVector(start + 3 , 8);
            var neVector = GenerateVector(start + 4 , 8);
            var eastVector = GenerateVector(start + 5 , 8);
            var seVector = GenerateVector(start + 6 , 8);
            var southVector = GenerateVector(start + 7 , 8);
            var swVector = GenerateVector(start + 8 , 8);

            _cardinalCrosses = eastVector.Concat(westVector).Concat(northVector).Concat(southVector).ToArray();
            _fixedCrosses = nwVector.Concat(neVector).Concat(seVector).Concat(swVector).ToArray();
        }

        private static List<double> GenerateVector(double start, int rounds)
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
                    lst.Add(next-1);
                }
                else
                {
                    lst.Add(next);
                }
            }

            return lst;
        }


        public bool AtCardinalCrossing(double longitude)
        {
            return _cardinalCrosses.Contains(longitude.TruncateDecimals());
        }

        public bool AtFixedCrossing(double longitude)
        {
            return _fixedCrosses.Contains(longitude.TruncateDecimals());
        }
    }
}
