using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Math
{
    public static class GilmoreGeometry
    {
        private static readonly int[] PrimaryStaticNumbers = { 34, 55, 89, 144, 233, 432, 377, 610, 987 };
        private static readonly int[] SecondaryStaticNumbers = { 29, 47, 76, 123, 199, 322, 521 };

        public static Dictionary<int, DateTime> GetPrimaryStaticDaysFromDate(DateTime date, int? limit = null)
        {
            return (limit.HasValue ? PrimaryStaticNumbers.Take(limit.Value) : PrimaryStaticNumbers)
                .ToDictionary(_ => _, _ => date.AddDays(_));
        }

        public static Dictionary<int, DateTime> GetSecondaryStaticDaysFromDate(DateTime date, int? limit = null)
        {
            return (limit.HasValue ? SecondaryStaticNumbers.Take(limit.Value) : SecondaryStaticNumbers)
                .ToDictionary(_ => _, _ => date.AddDays(_));
        }
    }
}
