using CycleFinder.Calculations.Math.Extremes;
using CycleFinder.Models;
using CycleFinder.Models.Astro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Astro.Extremes
{
    public class AstroExtremeCalculator : IAstroExtremeCalculator
    {
        private readonly IEphemerisEntryRepository _ephemerisEntryRepository;
        private readonly IInversionCalculator _inversionCalculator;

        public AstroExtremeCalculator(
            IEphemerisEntryRepository ephemerisEntryRepository,
            IInversionCalculator inversionCalculator)
        {
            _ephemerisEntryRepository = ephemerisEntryRepository;
            _inversionCalculator = inversionCalculator; 
        }

        public async Task<IEnumerable<Extreme>> GetExtremes(DateTime from, DateTime to, IEnumerable<Planet> planets, IEnumerable<ExtremeType> extremeTypes)
        {
            var ephem = await _ephemerisEntryRepository.GetEphemeris(from, to);
            List<Extreme> ret = new();


            foreach (var spl in planets)
            {
                foreach (var ext in extremeTypes)
                {
                    ret.AddRange(GetExtremes(ephem, spl, ext));
                }
            }

            return ret;
        }

        private IEnumerable<Extreme> GetExtremes(Ephemeris ephem, Planet planet, ExtremeType extremeType)
        {
            Func<Coordinates, double> selector = extremeType switch
            {
                ExtremeType.DeclinationMax or ExtremeType.DeclinationMin => c => c.Declination,
                ExtremeType.LatitudeMax or ExtremeType.LatitudeMin => c => c.Latitude,
                ExtremeType.SpeedMax or ExtremeType.SpeedMin => c => c.Speed,
                _ => throw new NotImplementedException()
            };

            Func<Ephemeris, Func<Coordinates, double>, Planet, IEnumerable<Coordinates>> extremeFunc = extremeType switch
            {
                ExtremeType.LatitudeMax or ExtremeType.SpeedMax or ExtremeType.DeclinationMax => GetMaximaBy,
                ExtremeType.LatitudeMin or ExtremeType.SpeedMin or ExtremeType.DeclinationMin => GetMinimaBy,
                _ => throw new NotImplementedException()
            };

            return extremeFunc(ephem, selector, planet).Select(c => new Extreme(c.EphemerisEntryTime, planet, selector(c), extremeType));
        }

        private IEnumerable<Coordinates> GetMinimaBy(Ephemeris ephem, Func<Coordinates, double> selector, Planet planet)
        {
            var coords = ephem.GetEntriesByPlanet(planet).ToArray();
            var indices = _inversionCalculator.FindMinima(coords.Select(selector).ToArray());
            return indices.Select(_ => coords[_]);
        }

        private IEnumerable<Coordinates> GetMaximaBy(Ephemeris ephem, Func<Coordinates, double> selector, Planet planet)
        {
            var coords = ephem.GetEntriesByPlanet(planet).ToArray();
            var indices = _inversionCalculator.FindMaxima(coords.Select(selector).ToArray());
            return indices.Select(_ => coords[_]);
        }
    }
}
