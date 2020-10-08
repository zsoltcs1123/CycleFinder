using CycleFinder.Models;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations
{
    public static class CandleStickMath
    {
        public static IEnumerable<CandleStick> GetLocalMinima(IEnumerable<CandleStick> data, int order)
        {
            if (data == null)
            {
                return null;
            }

            var arr = data.ToArray();
            var indices = GenericMath.FindLocalMinima(arr.Select(_ => _.Low).ToArray(), order);

            return indices.Select(_ => arr[_]);
        }

        public static IDictionary<CandleStick, IEnumerable<CandleStick>> GetTimeCyclesFromLows(IEnumerable<CandleStick> data, int order)
        {
            return GetLocalMinima(data, order)
                .ToDictionary(
                candle => candle, 
                candle => GilmoreGeometry.GetPrimaryStaticNumbersFromDate(candle.Time).Select(kvp => data.FirstOrDefault(c => c.Time == kvp.Value)));
        }
    }
}
