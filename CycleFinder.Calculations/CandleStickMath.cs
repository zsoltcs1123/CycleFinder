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

        public static IEnumerable<CandleStick> GetLocalMaxima(IEnumerable<CandleStick> data, int order)
        {
            if (data == null)
            {
                return null;
            }

            var arr = data.ToArray();
            var indices = GenericMath.FindLocalMaxima(arr.Select(_ => _.High).ToArray(), order);

            return indices.Select(_ => arr[_]);
        }

        public static IDictionary<CandleStick, IEnumerable<CandleStick>> GetPrimaryTimeCyclesFromLows(IEnumerable<CandleStick> data, int order)
        {
            //need to filter out dates that are in the future because the engine cannot draw only on candles
            //TODO find a way to draw in the future
            var maxDate = data.Last().Time;

            return GetLocalMinima(data, order)
                .ToDictionary(
                candle => candle, 
                candle => GilmoreGeometry.GetPrimaryStaticNumbersFromDate(candle.Time).Values
                .Where(date => date <= maxDate)
                .Select(date => data.FirstOrDefault(c => c.Time == date)));
        }

        public static IDictionary<CandleStick, IEnumerable<CandleStick>> GetPrimaryTimeCyclesFromHighs(IEnumerable<CandleStick> data, int order)
        {
            //need to filter out dates that are in the future because the engine cannot draw only on candles
            //TODO find a way to draw in the future
            var maxDate = data.Last().Time;

            return GetLocalMaxima(data, order)
                .ToDictionary(
                candle => candle,
                candle => GilmoreGeometry.GetPrimaryStaticNumbersFromDate(candle.Time).Values
                .Where(date => date <= maxDate)
                .Select(date => data.FirstOrDefault(c => c.Time == date)));
        }
    }
}
