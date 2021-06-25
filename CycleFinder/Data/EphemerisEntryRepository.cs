using CycleFinder.Calculations.Ephemeris;
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
        private readonly IAppCache _cache;


        public EphemerisEntryRepository(EphemerisEntryContext ephemerisEntryContext, IAppCache cache)
        {
            _ephemerisEntryContext = ephemerisEntryContext;
            _cache = cache;
        }

        public async Task<IEnumerable<EphemerisEntry>> GetEntries(DateTime startTime)
        {
            var earliestStartTime = _cache.GetOrAdd("EarliestEphemStartTime", () => startTime);

            var earliestEphemId = $"Ephem_{earliestStartTime}";

            if (startTime < earliestStartTime)
            {
                _cache.Remove(earliestEphemId);
                earliestStartTime = startTime;
            }
            
            var ret = await _cache.GetOrAddAsync(earliestEphemId, () => FilterByTime(earliestStartTime).ToListAsync());

            return startTime > earliestStartTime ? ret.Where(_ => _.Time >= startTime) : ret;
        }

        private IQueryable<EphemerisEntry> FilterByTime(DateTime startTime) => _ephemerisEntryContext.DailyEphemeris.Where(entry => entry.Time >= startTime);
    }
}
