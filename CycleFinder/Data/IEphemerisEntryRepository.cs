using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CycleFinder.Data
{
    public interface IEphemerisEntryRepository
    {
        Task<Ephemeris> GetEphemerisForPlanets(DateTime startTime, Planets planet);
        Task<IDictionary<DateTime, double>> GetDatesByLongitude(double longitude, Planets planet);
        Task<IDictionary<Planets, Coordinates>> GetCoordinatesByTime(DateTime time);
    }
}
