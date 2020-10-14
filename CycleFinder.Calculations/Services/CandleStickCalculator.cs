using CycleFinder.Models;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Services
{
    public class CandleStickCalculator : ICandleStickCalculator
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


        public IEnumerable<CandleWithTurns> GetPrimaryTimeCyclesFromLows(IEnumerable<CandleStick> data, int order)
        {
            //TODO find a way to draw in the future
            return GetLocalMinima(data, order)
                .Select(candle => 
                new CandleWithTurns(candle,GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time).Values
                .Select(date => data.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));
        }

        public IEnumerable<CandleWithTurns> GetPrimaryTimeCyclesFromHighs(IEnumerable<CandleStick> data, int order)
        {
            //TODO max search is incorrect (last and 1 candle before last is always high)
            return GetLocalMaxima(data, order)
                .Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time).Values
                .Select(date => data.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));
        }
    }
}
