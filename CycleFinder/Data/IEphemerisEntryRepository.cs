using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public interface IEphemerisEntryRepository
    {
        Task<Ephemerides> GetEphemeridesForPlanets(DateTime startTime, Planet planet);
    }
}
