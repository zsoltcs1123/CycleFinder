using CycleFinder.Calculations.Services;
using CycleFinder.Extensions;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using LazyCache;
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
        private readonly IAppCache _cache;

        private Func<Planets, EphemerisEntry, Coordinates> _planetSelector =
            (planet, entry) => planet switch
                {
                    Planets.Moon => entry.Moon,
                    Planets.Sun => entry.Sun,
                    Planets.Mercury => entry.Mercury,
                    Planets.Venus => entry.Venus,
                    Planets.Mars => entry.Mars,
                    Planets.Jupiter => entry.Jupiter,
                    Planets.Saturn => entry.Saturn,
                    Planets.Uranus => entry.Uranus,
                    Planets.Neptune => entry.Neptune,
                    Planets.Pluto => entry.Pluto,
                    _ => null,
                };
 

        public EphemerisEntryRepository(EphemerisEntryContext ephemerisEntryContext, ILongitudeComparer longitudeComparer, IAppCache cache)
        {
            _ephemerisEntryContext = ephemerisEntryContext;
            _longitudeComparer = longitudeComparer;
            _cache = cache;
        }

        public async Task<Coordinates> GetCoordinatesByTime(DateTime time, Planets planet)
        {
            return _planetSelector(planet, await _ephemerisEntryContext.DailyEphemeris.FirstOrDefaultAsync(_ => _.Time.Date == time.Date));
        }

        public async Task<IDictionary<Planets, Coordinates>> GetCoordinatesByTime(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<Ephemeris> GetEphemerisForPlanets(DateTime startTime, Planets planet)
        {
            return await _cache.GetOrAddAsync($"Coordinates_{(int)planet}_From{startTime}",
                async () => new Ephemeris(await _ephemerisEntryContext.DailyEphemeris.Where(entry => entry.Time >= startTime).ToDictionaryAsync(entry => entry.Time, _ => _planetSelector(planet, _)), planet));
        }

        public async Task<IDictionary<DateTime, double>> GetDatesByLongitude(double longitude, Planets planet)
        {
            return (await _ephemerisEntryContext.DailyEphemeris.ToDictionaryAsync(_ => _.Time, _ => _planetSelector(planet, _).Longitude))
                .Where(_ => _longitudeComparer.AreEqual(planet, longitude, _.Value))
                .ToDictionary(_ => _.Key, _ => _.Value);
        }
    }
}
