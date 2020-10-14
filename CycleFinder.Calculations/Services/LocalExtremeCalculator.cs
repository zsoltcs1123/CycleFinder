using CycleFinder.Models;
using CycleFinder.Models.Candles;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Services
{
    public class LocalExtremeCalculator : ILocalExtremeCalculator
    {
        public IEnumerable<CandleStick> GetLocalMinima(IEnumerable<CandleStick> data, int order)
        {
            if (data == null)
            {
                return null;
            }

            var arr = data.ToArray();
            var indices = GenericMath.FindLocalMinima(arr.Select(_ => _.Low).ToArray(), order);

            return indices.Select(_ => arr[_]);
        }

        public IEnumerable<CandleStick> GetLocalMaxima(IEnumerable<CandleStick> data, int order)
        {
            if (data == null)
            {
                return null;
            }

            var arr = data.ToArray();
            var indices = GenericMath.FindLocalMaxima(arr.Select(_ => _.High).ToArray(), order);

            return indices.Select(_ => arr[_]);
        }

        public IEnumerable<CandleStick> GetLocalExtremes(IEnumerable<CandleStick> data, int order)
        {
            if (data == null)
            {
                return null;
            }

            var arr = data.ToArray();
            var lowIndices = GenericMath.FindLocalMinima(arr.Select(_ => _.Low).ToArray(), order);
            var highIndices = GenericMath.FindLocalMaxima(arr.Select(_ => _.High).ToArray(), order);
            var indices = lowIndices.Concat(highIndices);

            return indices.Select(_ => arr[_]);
        }
    }
}
