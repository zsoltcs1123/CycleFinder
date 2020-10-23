using CycleFinder.Calculations.Extensions;
using CycleFinder.Calculations.Math;
using CycleFinder.Calculations.Services.Ephemeris;
using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using CycleFinder.Models.Markers;
using CycleFinder.Models.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services
{
    public class CandleStickMarkerCalculator : ICandleStickMarkerCalculator
    {
        private readonly Func<IRandomColorGenerator> _colorGeneratorFactory;
        private readonly ILongitudeComparer _longitudeComparer;
        private readonly ILocalExtremeCalculator _localExtremeCalculator;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;

        public CandleStickMarkerCalculator(
            ILongitudeComparer longitudeComparer, 
            ILocalExtremeCalculator localExtremeCalculator,
            IEphemerisEntryRepository ephemerisEntryRepository,
            Func<IRandomColorGenerator> colorGeneratorFactory)
        {
            _longitudeComparer = longitudeComparer;
            _localExtremeCalculator = localExtremeCalculator;
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _colorGeneratorFactory = colorGeneratorFactory;
        }

        public async Task<IEnumerable<ICandleStickMarker>> GetMarkers(CandleMarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
        {
            if (!spec.IsValid)
            {
                throw new Exception("Specification is not valid");
            }

            return spec switch
            {
                ExtremeCandleMarkerSpecification s  => await Task.Run(() => CreateExtremeMarkers(FilterExtremes(s.Extreme, candles, order, limit), s.Extreme)),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.Low => await Task.Run(() => CreateLowMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit))),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.High => await Task.Run(() => CreateHighMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit))),
                ExtremeCandleWithPlanetsMarkerSpecification s => await CreateExtremeMarkersWithPlanets(candles, FilterExtremes(s.Extreme, candles, order, limit), s),
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

        private async Task<IEnumerable<ICandleStickMarker>> CreateExtremeMarkersWithPlanets(IEnumerable<CandleStick> candles, IEnumerable<CandleStick> extremeCandles, ExtremeCandleWithPlanetsMarkerSpecification spec)
        {
            var cg = _colorGeneratorFactory();
            var ephem = await _ephemerisEntryRepository.GetEntries(candles.First().Time);

            if (!spec.IncludeLongitudinalReturns)
            {
                return extremeCandles
                    .Select(_ => new ExtremeCandleMarker(_, spec.Extreme, cg.GetRandomColor(), GetCoordinatesFromEphemerisEntry(ephem.FirstOrDefault(entry => entry.Time == _.Time))));
            }

            return null;

/*            var ret = new List<ICandleStickMarker>();
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
            return ret;*/
        }

        private Dictionary<Planet, Coordinates> GetCoordinatesForPlanets(Planet planets, Ephemerides ephemerides, DateTime time)
            => planets.GetFlags().ToDictionary(planet => planet, planet => ephemerides.Coordinates[time][planet]);

        private Dictionary<Planet, Coordinates> GetCoordinatesFromEphemerisEntry(EphemerisEntry entry)
        {
            return new Dictionary<Planet, Coordinates>
            {
                {Planet.Moon, entry.Moon },
                {Planet.Sun, entry.Sun },
                {Planet.Mercury, entry.Mercury },
                {Planet.Venus, entry.Venus },
                {Planet.Mars, entry.Mars },
                {Planet.Jupiter, entry.Jupiter },
                {Planet.Saturn, entry.Saturn },
                {Planet.Uranus, entry.Uranus },
                {Planet.Neptune, entry.Neptune },
                {Planet.Pluto, entry.Pluto },
            };
        }
    }
}
