using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations
{
    public static class GilmoreGeometry
    {
        private static readonly int[] PrimaryStaticNumbers = { 34, 55, 89, 144, 233, 432, 377, 610, 987 };
        private static readonly int[] SecondaryStaticNumbers = { 29, 47, 76, 123, 199, 322, 521 };

        public static Dictionary<int, DateTime> GetPrimaryStaticNumbersFromDate(DateTime date)
        {
            return PrimaryStaticNumbers.ToDictionary(_ => _, _ => date.AddDays(_));
        }

        public static Dictionary<int, DateTime> GetSecondaryStaticNumbersFromDate(DateTime date)
        {
            return SecondaryStaticNumbers.ToDictionary(_ => _, _ => date.AddDays(_));
        }
    }
}
