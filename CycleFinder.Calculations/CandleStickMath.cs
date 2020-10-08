using CycleFinder.Models;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations
{
    public static class CandleStickMath
    {
        public static IEnumerable<CandleStick> GetLocalMinima(IEnumerable<CandleStick> data)
        {
            if (data == null)
            {
                return null;
            }

            var arr = data.ToArray();
            var indices = GenericMath.FindLocalMinima(arr.Select(_ => _.Low).ToArray(), 5);

            return indices.Select(_ => arr[_]);
        }
    }
}
