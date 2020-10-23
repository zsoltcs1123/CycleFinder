using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Calculations.Services.Ephemeris
{
    public interface IEphemerisEntryRepository
    {
        Task<Ephemerides> GetEphemeridesForPlanets(DateTime startTime, Planet planet);

        Task<IEnumerable<EphemerisEntry>> GetEntries(DateTime startTime);
    }
}
