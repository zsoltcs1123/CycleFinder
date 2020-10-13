using CycleFinder.Calculations.Services;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public class EphemerisEntryRepository : IEphemerisEntryRepository
    {
        private readonly EphemerisEntryContext _ephemerisEntryContext;
        private readonly ILongitudeComparer _longitudeComparer;

        private Func<Planet, EphemerisEntry, Coordinates> _planetSelector =
            (planet, entry) => planet switch
                {
                    Planet.Moon => entry.Moon,
                    Planet.Sun => entry.Sun,
                    Planet.Mercury => entry.Mercury,
                    Planet.Venus => entry.Venus,
                    Planet.Mars => entry.Mars,
                    Planet.Jupiter => entry.Jupiter,
                    Planet.Saturn => entry.Saturn,
                    Planet.Uranus => entry.Uranus,
                    Planet.Neptune => entry.Neptune,
                    Planet.Pluto => entry.Pluto,
                    _ => null,
                };
 

        public EphemerisEntryRepository(EphemerisEntryContext ephemerisEntryContext, ILongitudeComparer longitudeComparer)
        {
            _ephemerisEntryContext = ephemerisEntryContext;
            _longitudeComparer = longitudeComparer;
        }

        public async Task<Coordinates> GetCoordinatesByTime(DateTime time, Planet planet)
        {
            return _planetSelector(planet, await _ephemerisEntryContext.Entries.FirstOrDefaultAsync(_ => _.Time.Date == time.Date));
        }

        public async Task<IDictionary<Planet, Coordinates>> GetCoordinatesByTime(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<IDictionary<DateTime, double>> GetDatesByLongitude(double longitude, Planet planet)
        {
            return (await _ephemerisEntryContext.Entries.ToDictionaryAsync(_ => _.Time, _ => _planetSelector(planet, _).Longitude))
                .Where(_ => _longitudeComparer.AreEqual(planet, longitude, _.Value))
                .ToDictionary(_ => _.Key, _ => _.Value);
        }
    }
}
