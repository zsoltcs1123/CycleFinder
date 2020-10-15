using CycleFinder.Calculations.Extensions;
using CycleFinder.Calculations.Services;
using CycleFinder.Extensions;
using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public class EphemerisEntryRepository : IEphemerisEntryRepository
    {
        private readonly EphemerisEntryContext _ephemerisEntryContext;
        private readonly ILongitudeComparer _longitudeComparer;
        private readonly IAppCache _cache;

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
 

        public EphemerisEntryRepository(EphemerisEntryContext ephemerisEntryContext, ILongitudeComparer longitudeComparer, IAppCache cache)
        {
            _ephemerisEntryContext = ephemerisEntryContext;
            _longitudeComparer = longitudeComparer;
            _cache = cache;
        }

        public async Task<Ephemerides> GetEphemeridesForPlanets(DateTime startTime, Planet planets)
        {
            var earliestStartTime = _cache.GetOrAdd($"int{planets}", () => startTime);

            if (startTime < earliestStartTime)
            {
                _cache.Remove($"Coordinates_{(int)planets}_From{earliestStartTime}");
            }

            return await _cache.GetOrAddAsync($"Coordinates_{(int)planets}_From{startTime}", async () => await GetEphemerides(startTime, planets));
        }

        private async Task<Ephemerides> GetEphemerides(DateTime startTime, Planet planets)
        {
            //TODO this can be a minor inefficiency since caching happens on the top level, e.g. a client asks for a Mercury ephemeris multiple times, it will be served from the cache
            //But if a client asks for a Mercury-Saturn ephemeris it will go to the DB first even though the Mercury data is already in the cache.
            //Architecture-wise this structure makes more sense to me so performance is sacrificed here.

            return new Ephemerides(await FilterByTime(startTime)
                .ToDictionaryAsync(entry => entry.Time, entry => planets.GetFlags().ToDictionary(planet => planet, planet => GetCoordinateFromEntry(entry, planet))));
        }

        private IQueryable<EphemerisEntry> FilterByTime(DateTime startTime) => _ephemerisEntryContext.DailyEphemeris.Where(entry => entry.Time >= startTime);

        private Coordinates GetCoordinateFromEntry(EphemerisEntry entry, Planet planet) => _planetSelector(planet, entry);
    }
}
