using CycleFinder.Calculations.Ephemeris;
using CycleFinder.Calculations.Ephemeris.Retrograde;
using CycleFinder.Calculations.Extensions;
using CycleFinder.Calculations.Math.Extremes;
using CycleFinder.Calculations.Math.Gilmore;
using CycleFinder.Calculations.Services;
using CycleFinder.Calculations.Services.Ephemeris.Aspects;
using CycleFinder.Models;
using CycleFinder.Models.Candles;
using CycleFinder.Models.Ephemeris;
using CycleFinder.Models.Extensions;
using CycleFinder.Models.Markers;
using CycleFinder.Models.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Markers
{
    public class CandleStickMarkerService : ICandleStickMarkerService
    {
        private readonly Func<IRandomColorGenerator> _colorGeneratorFactory;
        private readonly ILocalExtremeCalculator _localExtremeCalculator;
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly IAspectCalculator _aspectCalculator;
        private readonly IRetrogradeCalculcator _retrogradeCalculator;

        public CandleStickMarkerService(
            ILocalExtremeCalculator localExtremeCalculator,
            IEphemerisEntryRepository ephemerisEntryRepository,
            IAspectCalculator aspectCalculator,
            Func<IRandomColorGenerator> colorGeneratorFactory, IRetrogradeCalculcator retrogradeCalculator)
        {
            _localExtremeCalculator = localExtremeCalculator;
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _aspectCalculator = aspectCalculator;
            _colorGeneratorFactory = colorGeneratorFactory;
            _retrogradeCalculator = retrogradeCalculator;
        }


        public async Task<IEnumerable<ICandleStickMarker>> GetMarkers(MarkerSpecification spec)
        {
            if (!spec.IsValid)
            {
                throw new Exception("Specification is not valid");
            }

            return spec switch
            {
                AspectMarkerSpecification s => await GetAspectMarkers(s),
                RetrogradeMarkerSpecification s => await GetRetrogradeMarkers(s),
                _ => throw new NotImplementedException()
            };
        }


        private async Task<IEnumerable<EventMarker>> GetAspectMarkers(AspectMarkerSpecification spec)
        {
            return (await _aspectCalculator.GetAspects(spec.From, spec.Planet1, spec.Planet2, spec.AspectType)).Select(_ => new AspectMarker(_));
        }

        private async Task<IEnumerable<EventMarker>> GetRetrogradeMarkers(RetrogradeMarkerSpecification spec)
        {
            return (await _retrogradeCalculator.GetRetrogradeCycles(spec.Planet, spec.From)).RetrogradeStatusByDays
                .Select(_ => new RetrogradeMarker(_.Key, _.Value.RetrogradeStatus, _.Value.Coordinates.Longitude, _.Value.Coordinates.Speed));
        }

        public async Task<IEnumerable<ICandleStickMarker>> GetMarkers(MarkerSpecification spec, IEnumerable<CandleStick> candles, int order, int? limit)
        {
            if (!spec.IsValid)
            {
                throw new Exception("Specification is not valid");
            }

            return spec switch
            {
                ExtremeCandleMarkerSpecification s => await Task.Run(() => CreateExtremeMarkers(FilterExtremes(s.Extreme, candles, order, limit), s.Extreme)),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.Low => await Task.Run(() => CreateLowMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit), s)),
                ExtremeCandleWithTurnsMarkerSpecification s when s.Extreme == Extreme.High => await Task.Run(() => CreateHighMarkersWithTurns(FilterExtremes(s.Extreme, candles, order, limit), s)),
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

        private IEnumerable<ICandleStickMarker> CreateHighMarkersWithTurns(IEnumerable<CandleStick> highCandles, ExtremeCandleWithTurnsMarkerSpecification s)
        {
            var candlesWithTurns = highCandles.Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time, s.TurnsLimit).Values
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

        private IEnumerable<ICandleStickMarker> CreateLowMarkersWithTurns(IEnumerable<CandleStick> lowCandles, ExtremeCandleWithTurnsMarkerSpecification s)
        {
            var candlesWithTurns = lowCandles.Select(candle =>
                new CandleWithTurns(candle, GilmoreGeometry.GetPrimaryStaticDaysFromDate(candle.Time, s.TurnsLimit).Values
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
                    .Select(_ => new ExtremeCandleMarker(_, spec.Extreme, cg.GetRandomColor(), GetCoordinatesFromEphemerisEntry(ephem.FirstOrDefault(entry => entry.Time == _.Time), spec.Planets)));
            }

            //TODO
            return null;
        }


        private Dictionary<Planet, Coordinates> GetCoordinatesFromEphemerisEntry(EphemerisEntry entry, Planet planets)
        {
            var flags = planets.GetFlags();
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
            }.Where(_ => flags.Contains(_.Key)).ToDictionary(_ => _.Key, _ => _.Value);
        }
    }
}
