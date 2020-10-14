using CycleFinder.Calculations.Extensions;
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
        private readonly Func<IRandomColorGenerator> _colorGeneratorFactory;
        private readonly ILongitudeComparer _longitudeComparer;
        private readonly ILocalExtremeCalculator _localExtremeCalculator;

        public CandleStickMarkerCalculator(
            ILongitudeComparer longitudeComparer, 
            ILocalExtremeCalculator localExtremeCalculator,
            Func<IRandomColorGenerator> colorGeneratorFactory)
        {
            _longitudeComparer = longitudeComparer;
            _localExtremeCalculator = localExtremeCalculator;
            _colorGeneratorFactory = colorGeneratorFactory;
        }

        public IEnumerable<ICandleStickMarker> GetMarkers(CandleStickMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
        {
            var ret = new List<ICandleStickMarker>();
            if (spec.Extremes == Extremes.High)
            {
                if (spec.IncludeNone) 
                {
                    ret.AddRange(GenerateHighMarkers(FilterExtremes(Extremes.High, candles, order, limit)));
                    return ret;
                }

                if (spec.IncluePrimaryStaticCycles) ret.AddRange(GenerateHighMarkersWithTurns(FilterExtremes(Extremes.High, candles, order, limit)));
            }
            else if (spec.Extremes == Extremes.Low)
            {
                if (spec.IncludeNone) return GenerateLowMarkers(FilterExtremes(Extremes.Low, candles, order, limit));
                if (spec.IncluePrimaryStaticCycles) return GenerateLowMarkersWithTurns(FilterExtremes(Extremes.Low, candles, order, limit));
                else return null;
            }

            return ret;
        }

        private IEnumerable<CandleStick> FilterExtremes(Extremes extreme, IEnumerable<CandleStick> candles, int order, int? limit)
        {
            return extreme switch
            {
                Extremes.Low => _localExtremeCalculator.GetLocalMinima(candles, order).TakeLast(limit),
                Extremes.High => _localExtremeCalculator.GetLocalMaxima(candles, order).TakeLast(limit),
                _ => null,
            };
        }

        private IEnumerable<ICandleStickMarker> GenerateHighMarkers(IEnumerable<CandleStick> highCandles)
        {
            var cg = _colorGeneratorFactory();
            return highCandles.Select(_ => new HighCandleMarker(_, cg.GetRandomColor()));
        }

        private IEnumerable<ICandleStickMarker> GenerateLowMarkers(IEnumerable<CandleStick> lowCandles)
        {
            var cg = _colorGeneratorFactory();
            return lowCandles.Select(_ => new LowCandleMarker(_, cg.GetRandomColor()));
        }

        private IEnumerable<ICandleStickMarker> GenerateHighMarkersWithTurns(IEnumerable<CandleStick> highCandles)
        {
            var candlesWithTurns = highCandles.Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time).Values
                .Select(date => highCandles.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));

            var ret = new List<ICandleStickMarker>();
            var cg = _colorGeneratorFactory();
            int lowId = 1;
            foreach (var cwt in candlesWithTurns)
            {
                var color = cg.GetRandomColor();
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

        private IEnumerable<ICandleStickMarker> GenerateLowMarkersWithTurns(IEnumerable<CandleStick> lowCandles)
        {
            var candlesWithTurns = lowCandles.Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time).Values
                .Select(date => lowCandles.FirstOrDefault(c => c.Time == date) ?? new CandleStick(date.ToUnixTimestamp()))));

            var ret = new List<ICandleStickMarker>();
            var cg = _colorGeneratorFactory();

            int lowId = 1;
            foreach (var cwt in candlesWithTurns)
            {
                var color = cg.GetRandomColor();
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
    }
}
