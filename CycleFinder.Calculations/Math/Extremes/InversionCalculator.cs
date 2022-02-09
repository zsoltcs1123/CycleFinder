using System;
using System.Collections.Generic;

namespace CycleFinder.Calculations.Math.Extremes
{
    public class InversionCalculator : IInversionCalculator
    {
        private record struct DataWithSlope(double val, double slope, string direction);

        private enum InversionType
        {
            Min,
            Max,
            Both
        }

        private const string Ascending = "asc";
        private const string Descending = "desc";

        public IEnumerable<int> FindInversions(double[] arr)
        {
            return FindInversions(arr, InversionType.Both);
        }

        public IEnumerable<int> FindMinima(double[] arr)
        {
            return FindInversions(arr, InversionType.Min);
        }

        public IEnumerable<int> FindMaxima(double[] arr)
        {
            return FindInversions(arr, InversionType.Max);
        }

        private static List<int> FindInversions(double[] arr, InversionType inversionType)
        {
            DataWithSlope[] arrWithSlopes = new DataWithSlope[arr.Length];

            arrWithSlopes[0] = new DataWithSlope(arr[0], 0, "");
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arrWithSlopes[i + 1] = new DataWithSlope(arr[i + 1], (System.Math.Round(arr[i] - arr[i + 1], 2)) * -1, "");

                arrWithSlopes[i + 1].direction = CalculcateDirection(arrWithSlopes[i], arrWithSlopes[i + 1]);
            }

            return CheckDirection(arrWithSlopes);


            string CalculcateDirection(DataWithSlope current, DataWithSlope next)
            {
                if (next.slope > 0)
                {
                    return Ascending;
                }
                else if (next.slope == 0)
                {
                    if (current.direction == Ascending)
                        return Ascending;
                    else
                        return Descending;
                }
                else
                {
                    return Descending;
                }
            }

            List<int> CheckDirection(DataWithSlope[] data)
            {
                var ret = new List<int>();

                bool directionChecker(string s) => inversionType == InversionType.Both || s == (inversionType == InversionType.Max ? Ascending : Descending);

                for (int i = 1; i < data.Length - 1; i++)
                {
                    if (data[i].direction != data[i + 1].direction && directionChecker(data[i].direction))
                    {
                        ret.Add(i);
                    }
                }
                return ret;
            }
        }
    }
}
