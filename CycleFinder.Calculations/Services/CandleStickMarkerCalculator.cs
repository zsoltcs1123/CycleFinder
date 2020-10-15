using CycleFinder.Calculations.Extensions;
using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using CycleFinder.Models.Markers;
using CycleFinder.Models.Specifications;
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

        public IEnumerable<ICandleStickMarker> GetMarkers(CandleMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
        {
            if (!spec.IsValid)
            {
                throw new Exception("Specification is not valid");
            }

            return spec switch
            {
                ExtremeCandleMarkerSpecification s when s.Extreme == Extreme.Low => CreateLowCandleMarkers(FilterExtremes(s.Extreme, candles, order, limit)),
                ExtremeCandleMarkerSpecification s when s.Extreme == Extreme.High => CreateHighCandleMarkers(FilterExtremes(s.Extreme, candles, order, limit)),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.Low => CreateLowMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit)),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.High => CreateHighMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit)),
                ExtremeCandleWithPlanetsMarkerSpecification s when s.Extreme == Extreme.High => CreateLowMarkersWithPlanets(FilterExtremes(s.Extreme, candles, order, limit), s.Ephemerides, s.Planets),
                _ => null,
            };
        }


        private IEnumerable<CandleStick> FilterExtremes(Extreme extreme, IEnumerable<CandleStick> candles, int order, int? limit)
        {
            return extreme switch
            {
                Extreme.Low => _localExtremeCalculator.GetLocalMinima(candles, order).TakeLast(limit),
                Extreme.High => _localExtremeCalculator.GetLocalMaxima(candles, order).TakeLast(limit),
                _ => throw new NotImplementedException(),
            };
        }

        private IEnumerable<ICandleStickMarker> CreateHighCandleMarkers(IEnumerable<CandleStick> highCandles)
        {
            var cg = _colorGeneratorFactory();
            return highCandles.Select(_ => new HighCandleMarker(_, cg.GetRandomColor()));
        }

        private IEnumerable<ICandleStickMarker> CreateLowCandleMarkers(IEnumerable<CandleStick> lowCandles)
        {
            var cg = _colorGeneratorFactory();
            return lowCandles.Select(_ => new LowCandleMarker(_, cg.GetRandomColor()));
        }

        private IEnumerable<ICandleStickMarker> CreateHighMarkersWithTurns(IEnumerable<CandleStick> highCandles)
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

        private IEnumerable<ICandleStickMarker> CreateLowMarkersWithTurns(IEnumerable<CandleStick> lowCandles)
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

        private IEnumerable<ICandleStickMarker> CreateLowMarkersWithPlanets(IEnumerable<CandleStick> lowCandles, Ephemerides ephemerides, Planet planets)
        {
            var cg = _colorGeneratorFactory();
            return lowCandles.Select(_ => new LowCandleMarker(_, cg.GetRandomColor(), GetCoordinatesForPlanets(planets, ephemerides, _.Time)));
        }

        private Dictionary<Planet, Coordinates> GetCoordinatesForPlanets(Planet planets, Ephemerides ephemerides, DateTime time)
            => planets.GetFlags().ToDictionary(planet => planet, planet => ephemerides.Coordinates[time][planet]);
    }
}
