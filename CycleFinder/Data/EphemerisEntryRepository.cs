using CycleFinder.Calculations.Astro;
using CycleFinder.Models.Astro;
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
        private readonly IAppCache _cache;


        public EphemerisEntryRepository(EphemerisEntryContext ephemerisEntryContext, IAppCache cache)
        {
            _ephemerisEntryContext = ephemerisEntryContext;
            _cache = cache;
        }

        public async Task<IEnumerable<EphemerisEntry>> GetEntries(DateTime from, DateTime to)
        {
            if (to < from)
            {
                throw new Exception("End date must be later than start date");
            }
            
            var earliestStartTime = _cache.GetOrAdd("EarliestEphemStartTime", () => from);

            var earliestEphemId = $"Ephem_{earliestStartTime}";

            if (from < earliestStartTime)
            {
                _cache.Remove(earliestEphemId);
                earliestStartTime = from;
            }
            
            var ret = await _cache.GetOrAddAsync(earliestEphemId, () => FilterByTime(earliestStartTime, to).ToListAsync());

            return from > earliestStartTime ? ret.Where(_ => _.Time >= from) : ret;
        }

        public async Task<Ephemeris> GetEphemeris(DateTime from, DateTime to)
        {
            return new Ephemeris(await GetEntries(from, to));
        }

        private IQueryable<EphemerisEntry> FilterByTime(DateTime from, DateTime to) 
            => _ephemerisEntryContext.DailyEphemeris.Where(entry => entry.Time >= from && entry.Time <= to);
    }
}
