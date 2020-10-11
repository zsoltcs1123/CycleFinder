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

        public static IEnumerable<CandleWithTurns> GetPrimaryTimeCyclesFromLows(IEnumerable<CandleStick> data, int order)
        {
            //need to filter out dates that are in the future because the engine cannot draw only on candles
            //TODO find a way to draw in the future
            var maxDate = data.Last().Time;

            return GetLocalMinima(data, order)
                .Select(candle => 
                new CandleWithTurns(candle,GilmoreGeometry.GetPrimaryStaticNumbersFromDate(candle.Time).Values
                .Select(date => data.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));
        }

        public static IEnumerable<CandleWithTurns> GetPrimaryTimeCyclesFromHighs(IEnumerable<CandleStick> data, int order)
        {
            //need to filter out dates that are in the future because the engine cannot draw only on candles
            //TODO find a way to draw in the future
            var maxDate = data.Last().Time;

            //TODO max search is incorrect (last and 1 candle before last is always high)
            return GetLocalMaxima(data, order)
                .Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticNumbersFromDate(candle.Time).Values
                .Select(date => data.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));
        }
    }
}
