using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Markers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Calculations.Services
{
    public class CandleStickMarkerCalculator : ICandleStickMarkerCalculator
    {
        public IEnumerable<ICandleStickMarker> GetMarkers(CandleStickMarkerSpecification spec)
        {
            if (spec.Extremes == Extremes.High)
            {
                if (spec.IncludeNone) return GenerateHighMarkers(spec);
                if (spec.IncluePrimaryStaticCycles) return GenerateHighMarkersWithTurns(spec);
                else return null;
            }
            else if (spec.Extremes == Extremes.Low && spec.IncludeNone)
            {
                if (spec.IncludeNone) return GenerateLowMarkers(spec);
                if (spec.IncluePrimaryStaticCycles) return GenerateLowMarkersWithTurns(spec);
                else return null;
            }

            else return null;
        }

        private IEnumerable<ICandleStickMarker> GenerateHighMarkers(CandleStickMarkerSpecification spec)
        {
            return spec.Candles.Select(_ => new HighCandleMarker(_, spec.RandomColorGenerator.GetRandomColor()));
        }

        private IEnumerable<ICandleStickMarker> GenerateLowMarkers(CandleStickMarkerSpecification spec)
        {
            return spec.Candles.Select(_ => new LowCandleMarker(_, spec.RandomColorGenerator.GetRandomColor()));
        }

        private IEnumerable<ICandleStickMarker> GenerateHighMarkersWithTurns(CandleStickMarkerSpecification spec)
        {
            var candlesWithTurns = spec.Candles.Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time).Values
                .Select(date => spec.Candles.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));

            var ret = new List<ICandleStickMarker>();
            int lowId = 1;
            foreach (var cwt in candlesWithTurns)
            {
                var color = spec.RandomColorGenerator.GetRandomColor();
                ret.Add(new LowCandleMarker(cwt.Candle, color, lowId));

                int turnId = 1;
                foreach (var turn in cwt.StaticTurns)
                {
                    ret.Add(new HighCandleMarker(turn, color, lowId, turnId));
                    turnId++;
                }
                lowId++;
            }
            return ret;
        }

        private IEnumerable<ICandleStickMarker> GenerateLowMarkersWithTurns(CandleStickMarkerSpecification spec)
        {
            var candlesWithTurns = spec.Candles.Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time).Values
                .Select(date => spec.Candles.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));

            var ret = new List<ICandleStickMarker>();
            int lowId = 1;
            foreach (var cwt in candlesWithTurns)
            {
                var color = spec.RandomColorGenerator.GetRandomColor();
                ret.Add(new HighCandleMarker(cwt.Candle, color, lowId));

                int turnId = 1;
                foreach (var turn in cwt.StaticTurns)
                {
                    ret.Add(new LowCandleMarker(turn, color, lowId, turnId));
                    turnId++;
                }
                lowId++;
            }
            return ret;
        }
    }
}
