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
                ExtremeCandleMarkerSpecification s  => CreateExtremeMarkers(FilterExtremes(s.Extreme, candles, order, limit), s.Extreme),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.Low => CreateLowMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit)),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.High => CreateHighMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit)),
                ExtremeCandleWithPlanetsMarkerSpecification s => CreateExtremeMarkersWithPlanets(candles, FilterExtremes(s.Extreme, candles, order, limit), s),
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

        private IEnumerable<ICandleStickMarker> CreateExtremeMarkers(IEnumerable<CandleStick> highCandles, Extreme type)
        {
            var cg = _colorGeneratorFactory();
            return highCandles.Select(_ => new ExtremeCandleMarker(_, type, cg.GetRandomColor()));
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
                ret.Add(new ExtremeCandleMarker(cwt.Candle, Extreme.High, color, lowId));

                int turnId = 1;
                foreach (var turn in cwt.Turns)
                {
                    ret.Add(new ExtremeCandleMarker(turn, Extreme.Low, color, lowId, turnId));
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
                ret.Add(new ExtremeCandleMarker(cwt.Candle, Extreme.Low, color, lowId));

                int turnId = 1;
                foreach (var turn in cwt.Turns)
                {
                    ret.Add(new ExtremeCandleMarker(turn, Extreme.High, color, lowId, turnId));
                    turnId++;
                }
                lowId++;
            }
            return ret;
        }

        private IEnumerable<ICandleStickMarker> CreateExtremeMarkersWithPlanets(IEnumerable<CandleStick> candles, IEnumerable<CandleStick> extremeCandles, ExtremeCandleWithPlanetsMarkerSpecification spec)
        {
            var cg = _colorGeneratorFactory();

            if (!spec.IncludeLongitudinalReturns)
            {
                return extremeCandles.Select(_ => new ExtremeCandleMarker(_, spec.Extreme, cg.GetRandomColor(), GetCoordinatesForPlanets(spec.Planets, spec.Ephemerides, _.Time)));
            }

            var ret = new List<ICandleStickMarker>();
            foreach (var candle in candles)
            {
                var color = cg.GetRandomColor();
                var coordinatesPerPlanet = GetCoordinatesForPlanets(spec.Planets, spec.Ephemerides, candle.Time);
                ret.Add(new ExtremeCandleMarker(candle, spec.Extreme, color, coordinatesPerPlanet));


                foreach (var kvp in coordinatesPerPlanet)
                {
                    var planet = kvp.Key;
                    var coordinates = kvp.Value;

                    var nextReturn = spec.Ephemerides.Coordinates.FirstOrDefault(_ => _longitudeComparer.AreEqual(planet, _.Value[planet].Longitude, coordinates.Longitude));
                    var nextCandle = candles.FirstOrDefault(_ => _.Time == nextReturn.Key);

                    ret.Add(new LongitudinalReturnMarker(nextCandle, color, planet, coordinates.Longitude));
                }

            }
            //TODO consolidate markers i.e if the dates are the same, their texts should be appended
            return ret;
        }

        private Dictionary<Planet, Coordinates> GetCoordinatesForPlanets(Planet planets, Ephemerides ephemerides, DateTime time)
            => planets.GetFlags().ToDictionary(planet => planet, planet => ephemerides.Coordinates[time][planet]);
    }
}
